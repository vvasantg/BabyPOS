using BabyPOS_API.Data;
using BabyPOS_API.Models;
using Microsoft.EntityFrameworkCore;

namespace BabyPOS_API.Infrastructure.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _context;
        public MenuItemRepository(AppDbContext context) { _context = context; }

        public async Task<MenuItem> AddAsync(MenuItem item)
        {
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<MenuItemWithShopDto>> GetByShopWithShopNameAsync(int shopId)
        {
            return await _context.MenuItems
                .Where(m => m.ShopId == shopId)
                .Include(m => m.Shop)
                .Select(m => new MenuItemWithShopDto {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    ShopId = m.ShopId,
                    ShopName = m.Shop != null ? m.Shop.Name : string.Empty,
                    Category = (int)m.Category,
                    ImagePath = m.ImagePath
                })
                .ToListAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task<MenuItem?> UpdateAsync(int id, MenuItem item)
        {
            var existing = await _context.MenuItems.FindAsync(id);
            if (existing == null) return null;
            existing.Name = item.Name;
            existing.Price = item.Price;
            existing.Category = item.Category;
            existing.ImagePath = item.ImagePath;
            // ไม่ควรอัปเดต ShopId ในการแก้ไขเมนูอาหารปกติ
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return false;
            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
