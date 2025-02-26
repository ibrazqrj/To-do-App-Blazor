using Microsoft.AspNetCore.Components;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorToDoApp.Services
{
    public class LanguageService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IJSRuntime _jsRuntime;

        public CultureInfo CurrentCulture { get; private set; }

        public LanguageService(NavigationManager navigationManager, IJSRuntime jsRuntime)
        {
            _navigationManager = navigationManager;
            _jsRuntime = jsRuntime;
            CurrentCulture = CultureInfo.DefaultThreadCurrentCulture ?? new CultureInfo("en-US");
        }

        public async Task SetLanguage(string culture)
        {
            await _jsRuntime.InvokeVoidAsync("blazorCulture.set", culture);
            SetCulture(culture, true);
        }

        private void SetCulture(string culture, bool reloadPage)
        {
            var newCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentCulture = newCulture;
            CultureInfo.DefaultThreadCurrentUICulture = newCulture;
            CurrentCulture = newCulture;

            Console.WriteLine($"Sprache wurde gesetzt auf: {newCulture.Name}");

            if (reloadPage)
            {
                _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
            }
        }
    }
}
