using BabyPOS_API.Models;

namespace BabyPOS_API.Infrastructure.Repositories
{
    public interface IOrderRepository
    {
        Task<Order> AddAsync(Order order);
        Task<IEnumerable<Order>> GetByTableAsync(int tableId);
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> CloseAsync(int id);
        Task<bool> DeleteAsync(int id);
    }
}
