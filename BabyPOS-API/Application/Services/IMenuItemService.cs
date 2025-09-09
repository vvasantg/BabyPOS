using BabyPOS_API.Models;

namespace BabyPOS_API.Application.Services
{
    public interface IMenuItemService
    {
    Task<MenuItem> CreateMenuItemAsync(MenuItem item);
    Task<IEnumerable<MenuItemWithShopDto>> GetMenuItemsByShopWithShopNameAsync(int shopId);
        Task<MenuItem?> GetMenuItemAsync(int id);
        Task<MenuItem?> UpdateMenuItemAsync(int id, MenuItem item);
        Task<bool> DeleteMenuItemAsync(int id);
    }
}
