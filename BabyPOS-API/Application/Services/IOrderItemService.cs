using BabyPOS_API.Models;

namespace BabyPOS_API.Application.Services
{
    public interface IOrderItemService
    {
        Task<OrderItem> AddOrderItemAsync(OrderItem item);
        Task<IEnumerable<OrderItem>> GetOrderItemsByOrderAsync(int orderId);
        Task<OrderItem?> GetOrderItemAsync(int id);
        Task<OrderItem?> UpdateOrderItemAsync(int id, OrderItem item);
        Task<bool> DeleteOrderItemAsync(int id);
    }
}
