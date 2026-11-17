using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Payments;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace DalilAcademy.Infra.Payments
{
    public class PayPalPaymentGateway(HttpClient http, IOptions<PayPalSetting> options) : IPaymentGateway
    {
        private async Task<string> GetAccessTokenAsync()
        {
            // to implement Basic Auth, combine the username and the password, encode it to Base64, then send it in the header
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.Value.ClientId}:{options.Value.SecretKey}")); // Basic Auth format: Base64-encode "ClientId:SecretKey" (colon-separated)... note: Base64 is encoding, not encryption.... HTTPS is what protects it.
            var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", auth); // "Basic" is a name of an HTTP Authentication Scheme. It's one of several schemes (Bearer, Digest, etc.) used to prove identity to a server.
            request.Content = new FormUrlEncodedContent([new KeyValuePair<string, string>("grant_type", "client_credentials")]); // PayPal expects form-url-encoded data, not JSON... FormUrlEncodedContent was used bcs /v1/oauth2/token uses Content-Type: application/x-www-form-urlencoded
            // grant_type is the name of the authentication method... client_credentials is its value, its used for server-to-server communication with no user involved (the server provides the client id and the secret, in return of the token)

            var response = await http.SendAsync(request); // send payment request to paypal
            response.EnsureSuccessStatusCode(); // throw an exception if the status code is not 2xx

            var json = await response.Content.ReadFromJsonAsync<JsonElement>(); // read the response body and parse it as raw JSON
            // JsonElement will allow me to navigate the JSON dynamically e.g: GetProperty("name") is like accessing a dictionary key

            return json.GetProperty("access_token").GetString()!; // extract the "access_token" property from the JSON
        }

        public async Task<CreatePayPalOrderResponseDto> CreateOrderAsync(decimal amountUsd) // the payment does not occur yet, the method returns an approval URL where the user must go to log in and pay
        {
            var token = await GetAccessTokenAsync();

            var orderBody = new
            {
                intent = "CAPTURE", // "CAPTURE" means we intend to charge the user immediately upon approval
                purchase_units = new[] { // contains an array of the items to be purchased
                    new {
                        amount = new {
                            currency_code = "USD",
                            value = amountUsd.ToString("F2") // PayPal requires a string, not a number
                        }
                    }
                },
                payment_source = new
                {
                    paypal = new
                    {
                        experience_context = new
                        {
                            return_url = options.Value.ReturnUrl,
                            cancel_url = options.Value.ReturnUrl
                        }
                    }
                }
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "/v2/checkout/orders"); // Create a new HTTP POST request to create an order
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token); // Attach the Bearer token we got from GetAccessTokenAsync
            request.Content = JsonContent.Create(orderBody); // convert 'orderBody' object into JSON to add it to the request body.... JsonContent was used here because /v2/checkout/orders uses Content-Type: application/json

            var response = await http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var orderId = json.GetProperty("id").GetString()!;

            string? approvalUrl = null;
            foreach (var link in json.GetProperty("links").EnumerateArray()) // the response contains a "links" array. We need to iterate through it to find the approval URL...  EnumerateArray() allows us to loop through JSON array elements
            {
                var rel = link.GetProperty("rel").GetString();
                if (rel == "approve" || rel == "payer-action") // if the relationship is "approve" or "payer-action", this is the URL we want.
                {
                    approvalUrl = link.GetProperty("href").GetString(); // extract the "href" property, which is the actual URL.
                    break;
                }
            }

            if (string.IsNullOrEmpty(approvalUrl))  // If we didn't find an approval URL, smt went wrong.
                throw new InvalidOperationException("PayPal did not return an approval link.");

            return new CreatePayPalOrderResponseDto(orderId, approvalUrl);
        }

        public async Task<bool> CaptureOrderAsync(string orderId)
        {
            var token = await GetAccessTokenAsync();

            var request = new HttpRequestMessage(HttpMethod.Post, $"/v2/checkout/orders/{orderId}/capture");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(new { }); // capture endpoint does not require a body, but PayPal accepts empty JSON
            var response = await http.SendAsync(request);

            // ensureSuccessStatusCode() was not used here because we want to handle the failure
            if (!response.IsSuccessStatusCode) // if the request failed
                return false;

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var status = json.GetProperty("status").GetString();
            return status == "COMPLETED";
        }
    }
}
