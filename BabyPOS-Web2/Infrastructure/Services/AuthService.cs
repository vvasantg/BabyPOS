using BabyPOS_Web2.Application.DTOs;
using BabyPOS_Web2.Application.Services;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BabyPOS_Web2.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJSRuntime _jsRuntime;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime)
        {
            _httpClientFactory = httpClientFactory;
            _jsRuntime = jsRuntime;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                using var httpClient = _httpClientFactory.CreateClient("BabyPOS-API");
                var response = await httpClient.PostAsJsonAsync("api/users/login", loginDto, _jsonOptions);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<LoginApiResponse>(content, _jsonOptions);
                    
                    if (loginResult != null && !string.IsNullOrEmpty(loginResult.Token))
                    {
                        // Store token in localStorage
                        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwt", loginResult.Token);
                        
                        return new LoginResultDto
                        {
                            IsSuccess = true,
                            Token = loginResult.Token,
                            User = new UserDto
                            {
                                Id = loginResult.User.Id,
                                Username = loginResult.User.Username,
                                Email = loginResult.User.Email ?? string.Empty
                            }
                        };
                    }
                }
                
                return new LoginResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid username or password"
                };
            }
            catch (Exception ex)
            {
                return new LoginResultDto
                {
                    IsSuccess = false,
                    ErrorMessage = $"Login failed: {ex.Message}"
                };
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwt");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            try
            {
                var token = await GetCurrentTokenAsync();
                if (string.IsNullOrEmpty(token))
                    return null;

                // Decode user info from JWT token (simple implementation)
                var payload = token.Split('.')[1];
                var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(PadBase64(payload)));
                var tokenData = JsonSerializer.Deserialize<JsonElement>(json);

                if (tokenData.TryGetProperty("unique_name", out var usernameProp))
                {
                    return new UserDto
                    {
                        Username = usernameProp.GetString() ?? string.Empty
                    };
                }
            }
            catch
            {
                // Token might be invalid, remove it
                await LogoutAsync();
            }
            
            return null;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var token = await GetCurrentTokenAsync();
            return !string.IsNullOrEmpty(token);
        }

        public string? GetCurrentToken()
        {
            // This is synchronous version, use with caution
            return null;
        }

        private async Task<string?> GetCurrentTokenAsync()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwt");
            }
            catch
            {
                return null;
            }
        }

        private string PadBase64(string base64)
        {
            int padding = 4 - (base64.Length % 4);
            if (padding < 4) base64 += new string('=', padding);
            return base64.Replace('-', '+').Replace('_', '/');
        }

        // Internal classes for API response mapping
        private class LoginApiResponse
        {
            public string Token { get; set; } = string.Empty;
            public LoginApiUser User { get; set; } = new LoginApiUser();
        }

        private class LoginApiUser
        {
            public int Id { get; set; }
            public string Username { get; set; } = string.Empty;
            public string? Email { get; set; }
        }
    }
}
