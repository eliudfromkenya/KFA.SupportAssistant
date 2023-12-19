using Ardalis.GuardClauses;
using Ardalis.ListStartupServices;
using Autofac;
using Autofac.Extensions.DependencyInjection;
//using FastEndpoints.AspVersioning;
//using FastEndpoints.Security;
using FastEndpoints.Swagger;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Globals.Classes;
using KFA.SupportAssistant.Infrastructure;
using KFA.SupportAssistant.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using MySqlConnector;
using Serilog;
using KFA.SupportAssistant.Web.Binders;
using Microsoft.Data.SqlClient;
using FastEndpoints.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//string? connectionString = builder.Configuration.GetConnectionString("SqliteConnection");
string? connectionString = builder.Configuration.GetConnectionString("MySQLConnection");
LocalCache.ConString = builder.Configuration.GetConnectionString("LiteDB");

LoggerConfiguration logConfig = logConfig = new LoggerConfiguration();
builder.Host.UseSerilog((_, config) => new LoggerConfiguration()
               .ReadFrom.Configuration(builder.Configuration)
               .WriteTo.MySQL(
                   connectionString: connectionString,
                   tableName: "tbl_system_logs"));

Log.Logger = logConfig.CreateBootstrapLogger();

Log.Information("Starting the HostBuilder...");

builder.Services
   .AddCookieAuth(validFor: TimeSpan.FromMinutes(60))
   .AddJWTBearerAuth(builder.Configuration["Auth:TokenSigningKey"]!)
   .AddAuthentication(o =>
   {
     o.DefaultScheme = builder.Configuration["Auth:AuthScheme"];
     o.DefaultAuthenticateScheme = builder.Configuration["Auth:AuthScheme"];
   })
   .AddPolicyScheme(builder.Configuration["Auth:AuthScheme"]!, builder.Configuration["Auth:AuthScheme"], o =>
   {
     o.ForwardDefaultSelector = ctx =>
     {
       if (ctx.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authHeader) &&
           authHeader.FirstOrDefault()?.StartsWith("Bearer ") is true)
         return JwtBearerDefaults.AuthenticationScheme;
       return CookieAuthenticationDefaults.AuthenticationScheme;
     };
   });


Guard.Against.Null(connectionString);
var con = new MySqlConnection(connectionString);
builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseMySql(con, ServerVersion.AutoDetect(con)), ServiceLifetime.Scoped);
//builder.Services.AddMapster();

//builder.Services.AddSingleton(typeof(IRequestBinder<>), typeof(MyRequestBinder<>));
builder.Services.AddFastEndpoints()
                .AddAntiforgery();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.SwaggerDocument(o =>
{
  o.ShortSchemaNames = true;
  o.DocumentSettings = s =>
  {
    s.Title = "KFA Dynamics Support";
    s.Version = "V3.0.0.1";
  };
});

// add list services for diagnostic purposes - see https://github.com/ardalis/AspNetCoreStartupServices
builder.Services.Configure<ServiceConfig>(config =>
{
  config.Services = new List<ServiceDescriptor>(builder.Services);

  // optional - default path to view services is /listallservices - recommended to choose your own path
  config.Path = "/listservices";
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
  containerBuilder.RegisterModule(new DefaultCoreModule());
  containerBuilder.RegisterModule(new AutofacInfrastructureModule(builder.Environment.IsDevelopment()));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseShowAllServicesMiddleware(); // see https://github.com/ardalis/AspNetCoreStartupServices
}
else
{
  app.UseDefaultExceptionHandler(); // from FastEndpoints
  app.UseHsts();
}
app.UseAntiForgery()
   .UseFastEndpoints(c =>
   {
     c.Endpoints.RoutePrefix = "api/v3";
   });

app.UseSwaggerGen(); // FastEndpoints middleware

app.UseHttpsRedirection();

SeedDatabase(app);

app.Run();

static void SeedDatabase(WebApplication app)
{
  using var scope = app.Services.CreateScope();
  var services = scope.ServiceProvider;

  try
  {
    var context = services.GetRequiredService<AppDbContext>();
    //                    context.Database.Migrate();
    context.Database.EnsureCreated();
    SeedData.Initialize(services);
  }
  catch (Exception ex)
  {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred seeding the DB. {exceptionMessage}", ex.Message);
  }
}

// Make the implicit Program.cs class public, so integration tests can reference the correct assembly for host building
public partial class Program
{
}
