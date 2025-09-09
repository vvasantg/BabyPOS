using BabyPOS_Web2.Application.DTOs;

namespace BabyPOS_Web2.Application.Services
{
    public interface IMenuService
    {
        Task<List<MenuItemDto>> GetMenuItemsAsync(int shopId);
        Task<MenuItemDto?> GetMenuItemAsync(int id);
        Task<List<MenuItemDto>> GetMenuItemsByCategoryAsync(int shopId, int category);
    }
}
