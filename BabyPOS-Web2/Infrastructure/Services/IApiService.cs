using BabyPOS_Web2.Application.DTOs;

namespace BabyPOS_Web2.Infrastructure.Services
{
    public interface IApiService
    {
        Task<List<OrderDto>> GetOrdersAsync(int shopId);
        Task<List<OrderDto>> GetOrdersByStatusAsync(int shopId, string status);
        Task<OrderDto?> GetOrderAsync(int id);
        Task<OrderDto?> CreateOrderAsync(CreateOrderDto order);
        Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto statusUpdate);
        
        Task<List<MenuItemDto>> GetMenuItemsAsync(int shopId);
        Task<MenuItemDto?> GetMenuItemAsync(int id);
        
        Task<List<ShopDto>> GetShopsAsync();
        Task<ShopDto?> GetShopAsync(int id);
    Task<List<MenuItemDto>> GetShopMenuItemsAsync(int shopId);
    Task<List<TableDto>> GetTablesByShopAsync(int shopId);
    Task<bool> SubmitOrderAsync(object orderData);
    }
}
