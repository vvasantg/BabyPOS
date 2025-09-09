using BabyPOS_API.Models;

namespace BabyPOS_API.Application.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableId);
        Task<Order?> GetOrderAsync(int id);
        Task<Order?> CloseOrderAsync(int id);
        Task<bool> DeleteOrderAsync(int id);
    }
}
