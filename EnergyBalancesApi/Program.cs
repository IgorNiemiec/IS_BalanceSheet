using Microsoft.EntityFrameworkCore;
using EnergyBalancesApi.Data;       // DbContext
using EnergyBalancesApi.Models;     // modele, jeśli potrzebujesz
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;

using Polly;
using Polly.Extensions.Http;
using Refit;
using EnergyBalancesApi.Infrastructure.Clients;
using EnergyBalancesApi.Services;
using Newtonsoft.Json.Linq;
using EnergyBalancesApi.Models.Dto;
using EnergyBalancesApi.Services.FrontService;



static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}




var builder = WebApplication.CreateBuilder(args);

// Dodaj politykę do każdego klienta
builder.Services.AddHttpClient("Eurostat")
    .AddPolicyHandler(GetRetryPolicy());
builder.Services.AddHttpClient("OWID")
    .AddPolicyHandler(GetRetryPolicy());
builder.Services.AddHttpClient("IEA")
    .AddPolicyHandler(GetRetryPolicy());



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});



// SLOWNIK KRAJOW


// Przykład: prosty słownik w Program.cs
var countryLookup = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
{
    ["PL"] = 1,
    ["DE"] = 2,
    ["FR"] = 3,
};


builder.Services.AddSingleton<IDictionary<string, int>>(countryLookup);






// rejestracja DbContext
builder.Services.AddDbContext<EnergyDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// 4.1. Konfiguracja JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "EnergyBalancesApi API",
        Description = "Dokumentacja API EnergyBalancesApi"
    });
});


// Rejestracja HttpClientFactory
builder.Services.AddHttpClient();           // domyślny klient :contentReference[oaicite:3]{index=3}

// Eurostat – JSON‑stat format
builder.Services.AddHttpClient("Eurostat", c =>
{
    c.BaseAddress = new Uri("https://api.europa.eu/eurostat/data/");
    c.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});






builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddControllers();



builder.Services.AddHttpClient<IEurostatDataService, EurostatDataService>();
builder.Services.AddScoped<IDataTransformer, DataTransformer>();


builder.Services.AddScoped<EnergyQueryService>();
builder.Services.AddScoped<EnergyReportService>();
builder.Services.AddScoped<EnergyProductReportService>();



builder.Services.AddScoped<EnergyDataService>();

var application = builder.Build();

application.UseCors("AllowFrontend");
application.UseHttpsRedirection();


application.UseSwagger();              
application.UseSwaggerUI(c =>          
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EnergyBalancesApi v1");
    c.RoutePrefix = "swagger";  // np. ścieżka /swagger
});



application.UseAuthentication();
application.UseAuthorization();





var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

application.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

application.MapControllers();
application.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
