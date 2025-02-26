using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorToDoApp;
using BlazorToDoApp.Services;
using BlazorToDoApp.Handlers;
using BlazorToDoApp.Configuration;
using Syncfusion.Blazor;
using System.Globalization;
using Microsoft.JSInterop;
using MudBlazor.Services;
using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Root-Komponenten registrieren
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient-Instanz registrieren
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Microsoft Authentication einrichten
builder.Services.AddMsalAuthentication(options =>
{
    var conf = builder.Configuration;

    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add(conf.GetValue<string>(("Api:ApiAccess")));
    options.ProviderOptions.LoginMode = conf.GetValue<string>("Api:LoginMode");
});

// API-Settings abrufen und validieren
var apiSettings = builder.Configuration.GetSection("Api").Get<ApiSection>()
    ?? throw new Exception("Api settings not found");

// API-Authentifizierung einrichten
builder.Services.AddTransient<ApiAuthorizationMessageHandler>();

builder.Services.AddHttpClient("WebAPI", client =>
{
    client.BaseAddress = new Uri(apiSettings.BaseUrl);
}).AddHttpMessageHandler<ApiAuthorizationMessageHandler>();

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAPI"));


// Services für die Anwendung registrieren
builder.Services.AddScoped<HttpTodoService>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddMudServices(config =>
{
    var conf = builder.Configuration;

    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = conf.GetValue<bool>("MudBlazor:Snackbar:PreventDuplicates");
    config.SnackbarConfiguration.NewestOnTop = conf.GetValue<bool>("MudBlazor:Snackbar:NewestOnTop");
    config.SnackbarConfiguration.ShowCloseIcon = conf.GetValue<bool>("MudBlazor:Snackbar:ShowCloseIcon");
    config.SnackbarConfiguration.VisibleStateDuration = conf.GetValue<int>("MudBlazor:Snackbar:VisibleStateDuration");
    config.SnackbarConfiguration.HideTransitionDuration = conf.GetValue<int>("MudBlazor:Snackbar:HideTransitionDuration");
    config.SnackbarConfiguration.ShowTransitionDuration = conf.GetValue<int>("MudBlazor:Snackbar:ShowTransitionDuration");
    config.SnackbarConfiguration.SnackbarVariant = conf.GetValue<Variant>("MudBlazor:Snackbar:SnackbarVariant");
});

// Falls man HttpTodoService nutzen möchte:
builder.Services.AddScoped<ITodoService, HttpTodoService>();
// Falls man MemoryTodoService nutzen möchte:
//builder.Services.AddScoped<ITodoService, MemoryTodoService>();

// Localization einrichten
builder.Services.AddScoped<LanguageService>();

builder.Services.AddLocalization();

var host = builder.Build();
CultureInfo culture;
var js = host.Services.GetRequiredService<IJSRuntime>();
var result = await js.InvokeAsync<string>("blazorCulture.get");

if (result != null)
{
    culture = new CultureInfo(result);
}
else
{
    culture = new CultureInfo("en-US");
    await js.InvokeVoidAsync("blazorCulture.set", "en-US");
}

CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;


await host.RunAsync();


