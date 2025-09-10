using Microsoft.JSInterop;

namespace BabyPOS_Web2.Infrastructure.Services
{
    public interface IThemeService
    {
        Task<string> GetCurrentThemeAsync();
        Task SetThemeAsync(string theme);
        Task InitializeThemeAsync();
        event Action<string>? ThemeChanged;
    }

    public class ThemeService : IThemeService
    {
        private readonly IJSRuntime _jsRuntime;
        private string _currentTheme = "default";
        private bool _isPrerendering = true;

        public event Action<string>? ThemeChanged;

        public ThemeService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<string> GetCurrentThemeAsync()
        {
            try
            {
                if (_isPrerendering) return _currentTheme;
                
                var theme = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "babypos-theme");
                _currentTheme = string.IsNullOrEmpty(theme) ? "default" : theme;
                return _currentTheme;
            }
            catch
            {
                return "default";
            }
        }

        public async Task SetThemeAsync(string theme)
        {
            try
            {
                _currentTheme = theme;
                
                if (!_isPrerendering)
                {
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "babypos-theme", theme);
                    await ApplyThemeAsync(theme);
                }
                
                ThemeChanged?.Invoke(theme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting theme: {ex.Message}");
            }
        }

        public async Task InitializeThemeAsync()
        {
            try
            {
                _isPrerendering = false;
                var savedTheme = await GetCurrentThemeAsync();
                await ApplyThemeAsync(savedTheme);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing theme: {ex.Message}");
            }
        }

        private async Task ApplyThemeAsync(string theme)
        {
            try
            {
                if (_isPrerendering) return;
                
                var jsCode = $@"
                    const body = document.body;
                    
                    // Remove existing theme classes
                    body.classList.remove('theme-default', 'theme-pastel');
                    
                    // Add new theme class
                    if ('{theme}' === 'pastel') {{
                        body.classList.add('theme-pastel');
                    }} else {{
                        body.classList.add('theme-default');
                    }}
                ";

                await _jsRuntime.InvokeVoidAsync("eval", jsCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying theme: {ex.Message}");
            }
        }
    }
}
