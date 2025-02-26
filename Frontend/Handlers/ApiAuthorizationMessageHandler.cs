using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace BlazorToDoApp.Handlers;

public class ApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public ApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager,
        IConfiguration configuration) : base(provider, navigationManager)
    {
        string baseUrl = configuration["Api:BaseUrl"] ?? throw new Exception("Api:BaseUrl not found in configuration");
        ConfigureHandler([baseUrl]);
    }
}