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
        Task<List<OrderDto>> GetOrdersByShopAsync(int shopId);
        Task<bool> CloseOrderAsync(int orderId);
        
        Task<List<MenuItemDto>> GetMenuItemsAsync(int shopId);
        Task<MenuItemDto?> GetMenuItemAsync(int id);
        Task<MenuItemDto?> CreateMenuItemAsync(CreateMenuItemDto menuItem);
        Task<MenuItemDto?> UpdateMenuItemAsync(int id, CreateMenuItemDto menuItem);
        Task<bool> DeleteMenuItemAsync(int id);
        
        Task<List<ShopDto>> GetShopsAsync();
        Task<ShopDto?> GetShopAsync(int id);
        Task<ShopDto?> CreateShopAsync(ShopDto shop);
        Task<bool> UpdateShopAsync(ShopDto shop);
        Task<bool> DeleteShopAsync(int id);
        Task<List<ShopDto>> GetShopsForManagementAsync();
        
        Task<List<MenuItemDto>> GetShopMenuItemsAsync(int shopId);
        
        Task<List<TableDto>> GetTablesByShopAsync(int shopId);
        Task<TableDto?> GetTableAsync(int id);
        Task<TableDto?> CreateTableAsync(CreateTableDto table);
        Task<TableDto?> UpdateTableAsync(int id, CreateTableDto table);
        Task<bool> DeleteTableAsync(int id);
        
        // Bills
        Task<List<BillDto>> GetBillsByShopAsync(int shopId);
        Task<BillDto?> GetBillAsync(int id);
        Task<GenerateBillsDto?> GenerateBillsAsync(int shopId);
        Task<bool> PayBillAsync(int billId);
        
        Task<bool> SubmitOrderAsync(object orderData);
    }
}
