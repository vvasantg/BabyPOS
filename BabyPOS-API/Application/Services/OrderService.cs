using BabyPOS_API.Models;
using BabyPOS_API.Infrastructure.Repositories;

namespace BabyPOS_API.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;
        public OrderService(IOrderRepository repo) { _repo = repo; }

        public Task<Order> CreateOrderAsync(Order order) => _repo.AddAsync(order);
        public Task<IEnumerable<Order>> GetOrdersByTableAsync(int tableId) => _repo.GetByTableAsync(tableId);
        public Task<Order?> GetOrderAsync(int id) => _repo.GetByIdAsync(id);
        public Task<Order?> CloseOrderAsync(int id) => _repo.CloseAsync(id);
        public Task<bool> DeleteOrderAsync(int id) => _repo.DeleteAsync(id);
    }
}
