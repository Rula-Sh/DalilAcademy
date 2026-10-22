var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer(); // Important for minimal APIs + Swagger (lets the system discover all the API endpoints in the code)
builder.Services.AddSwaggerGen(options => // creates the file that describes the project APIs
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Dalil Academy LMS API",
        Version = "v1",
        Description = "Learning Management System API built with ASP.NET Core 10"
    });
});

// the above code is used to register the services that we need
var app = builder.Build(); // this line splits the project into two
// the below code manages the requests order.
/* Each request goes through stages (middlewares) (ex: one checks if the connection is safe, and the other checks the authorization, etc.)
   once the request passes those stages (middleware), it reaches the controller, which will do the work and return a response */

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => // helps showing Swagger UI
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Dalil Academy API v1");
        options.RoutePrefix = string.Empty; // Swagger at root
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection(); // converts every http request to https

app.UseAuthorization(); // checks the sender authorization, if his request is permissible or not

app.MapControllers(); // sends the request to the suitable controller

app.Run();
