using BabyPOS_API.Data;
using BabyPOS_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyPOS_API.Infrastructure.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;
        public OrderItemRepository(AppDbContext context) { _context = context; }

        public async Task<OrderItem> AddAsync(OrderItem item)
        {
            _context.OrderItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderAsync(int orderId)
        {
            return await _context.OrderItems.Where(i => i.OrderId == orderId).ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await _context.OrderItems.FindAsync(id);
        }

        public async Task<OrderItem?> UpdateAsync(int id, OrderItem item)
        {
            var existing = await _context.OrderItems.FindAsync(id);
            if (existing == null) return null;
            existing.MenuItemId = item.MenuItemId;
            existing.Quantity = item.Quantity;
            existing.Price = item.Price;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.OrderItems.FindAsync(id);
            if (item == null) return false;
            _context.OrderItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
