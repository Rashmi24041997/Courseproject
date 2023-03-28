using Courseproject.API;
using Courseproject.Business;
using Courseproject.Common.Interfaces;
using Courseproject.Common.Model;
using Courseproject.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Serilog;
using static System.Net.WebRequestMethods;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, provider, config) => config
.Enrich.FromLogContext()
.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, 
outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj} " +
"{Properties:j} {NewLine} {Exception}")
);

// Add services to the container.
DIConfiguration.RegisterServices(builder.Services);
var dbFilename = Environment.GetEnvironmentVariable("DB_FILENAME");
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlite($"Filename={dbFilename}"));
builder.Services.AddScoped<IGenericRepository<Address>, GenericRepository<Address>>();
builder.Services.AddScoped<IGenericRepository<Job>, GenericRepository<Job>>();
builder.Services.AddScoped<IGenericRepository<Employee>, GenericRepository<Employee>>();
builder.Services.AddScoped<IGenericRepository<Team>, GenericRepository<Team>>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUploadService, AzureBlobUploadService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var authUrl = Environment.GetEnvironmentVariable("AUTH_URL");
var tokenUrl = Environment.GetEnvironmentVariable("TOKEN_URL");
var oauthScope = Environment.GetEnvironmentVariable("SCOPE");

builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("oauth", new OpenApiSecurityScheme()
    {
        Description = "Auth Code Flow + PKCE",
        Name = "oauth",
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            AuthorizationCode = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri(authUrl),
                TokenUrl = new Uri(tokenUrl),
                Scopes = new Dictionary<string, string>()
                {
                    { oauthScope, "Access the API" }
                }
            }
        }
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        { 
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference(){ Type = ReferenceType.SecurityScheme, Id = "oauth"}
            },
            new List<string>(){ oauthScope}
        }
    });
});

var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
var adDomain = Environment.GetEnvironmentVariable("AD_DOMAIN");
var identityInstance = Environment.GetEnvironmentVariable("IDENTITY_INSTANCE");
var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
var callbackPath = Environment.GetEnvironmentVariable("CALLBACK_PATH");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi((bearerOptions) => { },
    (miOptions) =>
    {
        miOptions.ClientId = clientId;
        miOptions.ClientSecret = clientSecret;
        miOptions.Domain = adDomain;
        miOptions.Instance = identityInstance;
        miOptions.TenantId = tenantId;
        miOptions.CallbackPath = callbackPath;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.OAuthClientId(clientId);
    o.OAuthUsePkce();
    o.OAuthScopeSeparator(" ");
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
