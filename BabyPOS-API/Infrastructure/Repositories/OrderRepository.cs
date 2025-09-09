using BabyPOS_API.Data;
using BabyPOS_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyPOS_API.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context) { _context = context; }

        public async Task<Order> AddAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<IEnumerable<Order>> GetByTableAsync(int tableId)
        {
            return await _context.Orders.Where(o => o.TableId == tableId).ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Order?> CloseAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return null;
            order.IsClosed = true;
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
