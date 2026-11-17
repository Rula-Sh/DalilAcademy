namespace DalilAcademy.Infra.Payments
{
    public class PayPalSetting
    {
        public string ClientId { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public double ExpiresInHours { get; set; }
        public decimal JODToUSDRate { get; set; }
    }
}
