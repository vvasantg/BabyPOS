using BabyPOS_API.Models;
using BabyPOS_API.Infrastructure.Repositories;

namespace BabyPOS_API.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _repo;
        public MenuItemService(IMenuItemRepository repo) { _repo = repo; }

    public Task<MenuItem> CreateMenuItemAsync(MenuItem item) => _repo.AddAsync(item);
    public Task<IEnumerable<MenuItemWithShopDto>> GetMenuItemsByShopWithShopNameAsync(int shopId) => _repo.GetByShopWithShopNameAsync(shopId);
        public Task<MenuItem?> GetMenuItemAsync(int id) => _repo.GetByIdAsync(id);
        public Task<MenuItem?> UpdateMenuItemAsync(int id, MenuItem item) => _repo.UpdateAsync(id, item);
        public Task<bool> DeleteMenuItemAsync(int id) => _repo.DeleteAsync(id);
    }
}
