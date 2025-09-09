using BabyPOS_API.Models;
using BabyPOS_API.Infrastructure.Repositories;

namespace BabyPOS_API.Application.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _repo;
        public OrderItemService(IOrderItemRepository repo) { _repo = repo; }

        public Task<OrderItem> AddOrderItemAsync(OrderItem item) => _repo.AddAsync(item);
        public Task<IEnumerable<OrderItem>> GetOrderItemsByOrderAsync(int orderId) => _repo.GetByOrderAsync(orderId);
        public Task<OrderItem?> GetOrderItemAsync(int id) => _repo.GetByIdAsync(id);
        public Task<OrderItem?> UpdateOrderItemAsync(int id, OrderItem item) => _repo.UpdateAsync(id, item);
        public Task<bool> DeleteOrderItemAsync(int id) => _repo.DeleteAsync(id);
    }
}
