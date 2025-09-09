using BabyPOS_Web2.Application.DTOs;
using BabyPOS_Web2.Domain.Models;
using BabyPOS_Web2.Infrastructure.Services;

namespace BabyPOS_Web2.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IApiService _apiService;

        public MenuService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<MenuItemDto>> GetMenuItemsAsync(int shopId)
        {
            return await _apiService.GetMenuItemsAsync(shopId);
        }

        public async Task<MenuItemDto?> GetMenuItemAsync(int id)
        {
            return await _apiService.GetMenuItemAsync(id);
        }

        public async Task<List<MenuItemDto>> GetMenuItemsByCategoryAsync(int shopId, int category)
        {
            var allItems = await _apiService.GetMenuItemsAsync(shopId);
            return allItems.Where(item => (int)item.Category == category).ToList();
        }
    }
}
