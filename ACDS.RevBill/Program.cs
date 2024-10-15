using System.Text;
using System.Text.Json;
using ACDS.RevBill.Contracts;
using ACDS.RevBill.Entities.Email;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Extensions;
using ACDS.RevBill.Helpers;
using ACDS.RevBill.Presentation.ActionFilters;
using ACDS.RevBill.Services;
using Audit.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using static ACDS.RevBill.Extensions.ServiceExtensions;

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"revbill_log.txt")
                // uncomment to write to Azure diagnostics stream
                //.WriteTo.File(
                //    @"D:\home\LogFiles\Application\ProjectDigital_webhost_api_log.txt",
                //    fileSizeLimitBytes: 1_000_000,
                //    rollOnFileSizeLimit: true,
                //    shared: true,
                //    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

var config = builder.Configuration;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.ConfigureCors();
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
LogManager.ThrowExceptions = true;
LogManager.ThrowConfigExceptions = true;

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.ConfigureIISIntegration();
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureEmailSender();
builder.Services.ConfigureSqlContext(config);
builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection")));
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<Tenancy>(); // Register Tenancy as a scoped service
builder.Services.AddScoped<IQRCodeService,QrCodeService>();

//register email configuration
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

//register PID configuration
var pidConfig = builder.Configuration.GetSection("PID").Get<PID>();
builder.Services.AddSingleton(pidConfig);

builder.Services.AddControllers();

builder.Services.AddSingleton(new JwtHelper());

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "https://localhost:7180/",
        ValidIssuer = "https://localhost:7180/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tokenSecurityKey@1"))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            // Ensure we always have an error and error description.
            if (string.IsNullOrEmpty(context.Error))
                context.Error = "invalid_token";
            if (string.IsNullOrEmpty(context.ErrorDescription))
                context.ErrorDescription = "This request requires a valid JWT access token to be provided";

            // Add some extra context for expired tokens.
            if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
            {
                var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = context.Error,
                error_description = context.ErrorDescription,
                statusCode = StatusCodes.Status401Unauthorized,
            }));
        },
    };
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped<ValidationFilterAttribute>();
//To add QRcode


builder.Services.ConfigureVersioning();
builder.Services.ConfigureSwagger();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(ACDS.RevBill.Presentation.AssemblyReference).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

//AuditTrail configuration to use sql server
Configuration.Setup()
          .UseSqlServer(_ => _
             .ConnectionString(config.GetConnectionString("sqlConnection"))
             .Schema("dbo")
             .TableName("AuditTrail")
             .IdColumnName("EventId")
             .JsonColumnName("JsonData")
             .LastUpdatedColumnName("LastUpdatedDate")
             .CustomColumn("EventType", ev => ev.EventType)
             .CustomColumn("User", ev => ev.Environment.UserName));

//registers the JSONModelService dependency injection
builder.Services.AddScoped<JsonModelService>();

var app = builder.Build();

//var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler();

if (app.Environment.IsProduction())
    app.UseHsts();

// Audit Trail middleware
//app.UseMiddleware<AuditTrailMiddleware>();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

//add security headers
//app.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("Content-Security-Policy", new StringValues("default-src 'self'"));
//    context.Response.Headers.Add("X-Content-Type-Options", new StringValues("nosniff"));
//    context.Response.Headers.Add("X-Frame-Options", new StringValues("SAMEORIGIN"));
//    context.Response.Headers.Add("X-XSS-Protection", new StringValues("1; mode=block"));
//    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
//    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
//    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
//    context.Response.Headers.Add("Permissions-Policy", "geolocation=(), microphone=(), camera=()");

//    await next();
//});

app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);


app.UseHttpsRedirection();

app.UseStaticFiles();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "RevBill API v1");
});

app.MapControllers();

app.Run();