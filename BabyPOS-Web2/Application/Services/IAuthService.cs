using BabyPOS_Web2.Application.DTOs;

namespace BabyPOS_Web2.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync();
        Task<UserDto?> GetCurrentUserAsync();
        Task<bool> IsAuthenticatedAsync();
        string? GetCurrentToken();
    }
}
