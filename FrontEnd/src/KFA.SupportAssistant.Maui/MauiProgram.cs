using Blazored.LocalStorage;
using KFA.SupportAssistant.RCL.Data;
using KFA.SupportAssistant.RCL.Handlers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

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
          builder.Services.AddBlazoredLocalStorage();

#if DEBUG
    builder.Services.AddBlazorWebViewDeveloperTools();
  		builder.Logging.AddDebug();
#endif

          return builder.Build();
      }
  }
