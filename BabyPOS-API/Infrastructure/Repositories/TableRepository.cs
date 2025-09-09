using BabyPOS_API.Data;
using BabyPOS_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyPOS_API.Infrastructure.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly AppDbContext _context;
        public TableRepository(AppDbContext context) { _context = context; }

        public async Task<Table> AddAsync(Table table)
        {
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();
            return table;
        }

        public async Task<IEnumerable<Table>> GetByShopAsync(int shopId)
        {
            return await _context.Tables.Where(t => t.ShopId == shopId).ToListAsync();
        }

        public async Task<Table?> GetByIdAsync(int id)
        {
            return await _context.Tables.FindAsync(id);
        }

        public async Task<Table?> UpdateAsync(int id, Table table)
        {
            var existing = await _context.Tables.FindAsync(id);
            if (existing == null) return null;
            existing.Name = table.Name;
            existing.ShopId = table.ShopId;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null) return false;
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
