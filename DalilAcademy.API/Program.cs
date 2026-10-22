var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// the above code is used to register the services that we need
var app = builder.Build(); // this line splits the project into two
// the below code manages the requests order.
/* Each request goes through stages (middlewares) (ex: one checks if the connection is safe, and the other checks the authorization, etc.)
   once the request passes those stages (middleware), it reaches the controller, which will do the work and return a response */

app.Use(async (context, next) => // + added for demonstration
{
    Console.WriteLine($"Request in: {context.Request.Path}");
    await next();
    Console.WriteLine($"Response out: {context.Response.StatusCode}");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection(); // converts every http request to https

app.UseAuthorization(); // checks the sender authorization, if his request is permissible or not

app.MapControllers(); // sends the request to the suitable controller

app.Run();
