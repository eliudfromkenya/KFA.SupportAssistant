using Blazored.LocalStorage;
using KFA.SupportAssistant.RCL.Data;
using KFA.SupportAssistant.RCL.Handlers;
using KFA.SupportAssistant.RCL.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Fluxor;
using KFA.SupportAssistant.RCL.State.MainTitle;

namespace KFA.SupportAssistant.Maui;

  public static class MauiProgram
  {
      public static MauiApp CreateMauiApp()
      {
          var builder = MauiApp.CreateBuilder();
          builder
              .UseMauiApp<App>()
              .ConfigureFonts(fonts =>
              {
                  fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
              });

          builder.Services.AddMauiBlazorWebView();
          builder.Services.AddTransient<ValidateHeaderHandler>();
          builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
          builder.Services.AddSingleton<HttpClient>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddBlazoredLocalStorage();
    builder.Services.AddFluxor(o =>
    {
      o.ScanAssemblies(typeof(MauiProgram).Assembly, typeof(MainTitleState).Assembly);
      o.UseReduxDevTools(rdt =>
      {
        rdt.Name = "KFA Support Assistant";
      });
    });
    builder.Services.AddAuthorizationCore(options =>
    {
      options.AddPolicy("SeniorEmployee", policy =>
          policy.RequireClaim("IsUserEmployedBefore1990", "true"));
    });

#if DEBUG
    builder.Services.AddBlazorWebViewDeveloperTools();
  		//builder.Logging.AddDebug();
#endif

          return builder.Build();
      }
  }
