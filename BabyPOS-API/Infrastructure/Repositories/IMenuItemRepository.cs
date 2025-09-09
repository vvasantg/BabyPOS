using BabyPOS_API.Models;

namespace BabyPOS_API.Infrastructure.Repositories
{
    public interface IMenuItemRepository
    {
    Task<MenuItem> AddAsync(MenuItem item);
    Task<IEnumerable<MenuItemWithShopDto>> GetByShopWithShopNameAsync(int shopId);
        Task<MenuItem?> GetByIdAsync(int id);
        Task<MenuItem?> UpdateAsync(int id, MenuItem item);
        Task<bool> DeleteAsync(int id);
    }
}
