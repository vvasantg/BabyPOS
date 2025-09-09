using BabyPOS_API.Models;

namespace BabyPOS_API.Infrastructure.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItem> AddAsync(OrderItem item);
        Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId);
        Task<OrderItem?> GetByIdAsync(int id);
        Task<OrderItem?> UpdateAsync(int id, OrderItem item);
        Task<bool> DeleteAsync(int id);
    }
}
