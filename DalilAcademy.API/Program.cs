using DalilAcademy.API.Middleware;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using DalilAcademy.Core.Validators;
using DalilAcademy.Infra.Data;
using DalilAcademy.Infra.Repository;
using DalilAcademy.Infra.Security;
using DalilAcademy.Infra.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateEnrollmentDtoValidator>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

//var issuer = builder.Configuration["Jwt:Issuer"]; // i will need to also initialize audience & expiresInHours as well from appsettings.json, this is error prone, so its best to do it in a separate file which will be JwtSetting.cs
// the below code snippet teaches the application how to read the setting that we created it and how to let it check each token it recieves
var jwt = builder.Configuration.GetSection("jwt").Get<JwtSetting>(); // merges the key with the Jwt object in appsettings.json // allows me to use JwtSetting in program.cs
builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection("jwt")); // to registers JwtSetting in the DI so that eny other class needs it can use it in the constructor through IOptions
// activate the authentication & athorization using JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true, // did we issue the token (create it // the issuer of the token)?
    ValidateAudience = true, // is the token for the project audience?
    ValidateLifetime = true, // is the token expired or not?
    ValidateIssuerSigningKey = true, // is the signiture correct or not?
    ValidIssuer = jwt.Issuer,
    ValidAudience = jwt.Audience,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
    // IssuerSigningKey is the key that we used in the user secret 
    // SymmetricSecurityKey signes the token and checks it (the server can only do that)
    // Encoding.UTF8.GetBytes() converts the string to byte
    ClockSkew = TimeSpan.FromMinutes(1)  // the tocken is valid for 1 minute here after its expiration (used just incase the server time was different from the client side)
});
builder.Services.AddAuthorization();

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

app.UseExceptionHandler();

app.UseHttpsRedirection(); // converts every http request to https

app.UseAuthentication();

app.UseAuthorization(); // checks the sender authorization, if his request is permissible or not

app.MapControllers(); // sends the request to the suitable controller

app.Run();
