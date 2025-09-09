using BabyPOS_API.Data;
using BabyPOS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyPOS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopsController : ControllerBase
    {

        // GET: api/shops/manage - Get shops for the current owner (for management)
        [HttpGet("manage")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetShopsForOwner()
        {
            // Get user id from JWT claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid token: cannot get user id");

            var shops = await _context.Shops
                .Include(s => s.Owner)
                .Where(s => s.OwnerId == userId)
                .ToListAsync();

            // ดึงเมนูอาหาร 3 รายการแรกของแต่ละร้าน
            var shopIds = shops.Select(s => s.Id).ToList();
            var menuItems = await _context.MenuItems
                .Where(m => shopIds.Contains(m.ShopId))
                .ToListAsync();

            var result = shops.Select(shop => {
                var foods = new List<Models.MenuItemDto>();
                var shopMenus = menuItems.Where(m => m.ShopId == shop.Id).ToList();
                // เลือก MainDish ก่อน, Dessert, Drink
                var main = shopMenus.FirstOrDefault(m => m.Category == Models.FoodCategory.MainDish);
                if (main != null)
                    foods.Add(new Models.MenuItemDto { Id = main.Id, Name = main.Name, Price = main.Price, ImagePath = main.ImagePath });
                var dessert = shopMenus.FirstOrDefault(m => m.Category == Models.FoodCategory.Dessert && (main == null || m.Id != main.Id));
                if (dessert != null)
                    foods.Add(new Models.MenuItemDto { Id = dessert.Id, Name = dessert.Name, Price = dessert.Price, ImagePath = dessert.ImagePath });
                var drink = shopMenus.FirstOrDefault(m => m.Category == Models.FoodCategory.Drink && (main == null || m.Id != main.Id) && (dessert == null || m.Id != dessert.Id));
                if (drink != null)
                    foods.Add(new Models.MenuItemDto { Id = drink.Id, Name = drink.Name, Price = drink.Price, ImagePath = drink.ImagePath });
                // ถ้ายังไม่ครบ 3 ให้เติมเมนูอื่น ๆ
                if (foods.Count < 3)
                {
                    var others = shopMenus.Where(m => !foods.Any(f => f.Id == m.Id)).Take(3 - foods.Count);
                    foreach (var m in others)
                        foods.Add(new Models.MenuItemDto { Id = m.Id, Name = m.Name, Price = m.Price, ImagePath = m.ImagePath });
                }
                return new Models.ShopWithFoodsDto
                {
                    Id = shop.Id,
                    Name = shop.Name,
                    Foods = foods
                };
            }).ToList();
            return Ok(result);
        }
        private readonly AppDbContext _context;
        public ShopsController(AppDbContext context)
        {
            _context = context;
        }

        // สร้างร้านโดยใช้ user id จาก token
        [HttpPost]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> CreateShop([FromBody] Shop shop)
        {
            // ดึง user id จาก claims (sub หรือ NameIdentifier)
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid token: cannot get user id");

            shop.OwnerId = userId;
            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
            return Ok(shop);
        }

        // ?????????????
        [HttpGet]
        public async Task<IActionResult> GetShops()
        {
            var shops = await _context.Shops.Include(s => s.Owner).ToListAsync();

            // ดึงเมนูอาหาร 3 รายการแรกของแต่ละร้าน (เหมือน /manage)
            var shopIds = shops.Select(s => s.Id).ToList();
            var menuItems = await _context.MenuItems
                .Where(m => shopIds.Contains(m.ShopId))
                .ToListAsync();

            var result = shops.Select(shop => {
                var foods = new List<Models.MenuItemDto>();
                var shopMenus = menuItems.Where(m => m.ShopId == shop.Id).ToList();
                // เลือก MainDish ก่อน, Dessert, Drink
                var main = shopMenus.FirstOrDefault(m => m.Category == Models.FoodCategory.MainDish);
                if (main != null)
                    foods.Add(new Models.MenuItemDto { Id = main.Id, Name = main.Name, Price = main.Price, ImagePath = main.ImagePath });
                var dessert = shopMenus.FirstOrDefault(m => m.Category == Models.FoodCategory.Dessert && (main == null || m.Id != main.Id));
                if (dessert != null)
                    foods.Add(new Models.MenuItemDto { Id = dessert.Id, Name = dessert.Name, Price = dessert.Price, ImagePath = dessert.ImagePath });
                var drink = shopMenus.FirstOrDefault(m => m.Category == Models.FoodCategory.Drink && (main == null || m.Id != main.Id) && (dessert == null || m.Id != dessert.Id));
                if (drink != null)
                    foods.Add(new Models.MenuItemDto { Id = drink.Id, Name = drink.Name, Price = drink.Price, ImagePath = drink.ImagePath });
                // ถ้ายังไม่ครบ 3 ให้เติมเมนูอื่น ๆ
                if (foods.Count < 3)
                {
                    var others = shopMenus.Where(m => !foods.Any(f => f.Id == m.Id)).Take(3 - foods.Count);
                    foreach (var m in others)
                        foods.Add(new Models.MenuItemDto { Id = m.Id, Name = m.Name, Price = m.Price, ImagePath = m.ImagePath });
                }
                return new Models.ShopWithFoodsDto
                {
                    Id = shop.Id,
                    Name = shop.Name,
                    Foods = foods
                };
            })
            .Where(shopDto => shopDto.Foods != null && shopDto.Foods.Count > 0)
            .ToList();
            return Ok(result);
        }

        // ????????????????
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShop(int id)
        {
            var shop = await _context.Shops.Include(s => s.Owner).FirstOrDefaultAsync(s => s.Id == id);
            if (shop == null) return NotFound();
            return Ok(shop);
        }

        // ?????????
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShop(int id, [FromBody] Shop shop)
        {
            var existing = await _context.Shops.FindAsync(id);
            if (existing == null) return NotFound();
            existing.Name = shop.Name;
            existing.OwnerId = shop.OwnerId;
            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // ??????
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShop(int id)
        {
            var shop = await _context.Shops.FindAsync(id);
            if (shop == null) return NotFound();
            _context.Shops.Remove(shop);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
