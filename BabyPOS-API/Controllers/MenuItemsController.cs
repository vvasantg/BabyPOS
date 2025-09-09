using BabyPOS_API.Application.Services;
using BabyPOS_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyPOS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _service;
        private readonly BabyPOS_API.Data.AppDbContext _context;
        public MenuItemsController(IMenuItemService service, BabyPOS_API.Data.AppDbContext context)
        {
            _service = service;
            _context = context;
        }
        // GET: api/menuitems/manage - Get all menu items for shops owned by current user
        [HttpGet("manage")]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> GetMenuItemsForOwner()
        {
            // Get user id from JWT claims
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid token: cannot get user id");

            // Find all shop ids owned by this user
            var shopIds = await _context.Shops
                .Where(s => s.OwnerId == userId)
                .Select(s => s.Id)
                .ToListAsync();

            // Find all menu items for these shops
            var menuItems = await _context.MenuItems
                .Where(m => shopIds.Contains(m.ShopId))
                .ToListAsync();

            return Ok(menuItems);
        }

        // ??????????????????
        [HttpPost]
        public async Task<IActionResult> CreateMenuItem([FromBody] MenuItem item)
        {
            if (item.Price < 0)
                return BadRequest("Price must not be negative.");
            return Ok(await _service.CreateMenuItemAsync(item));
        }

        // ????????????????????
        [HttpGet("shop/{shopId}")]
        public async Task<IActionResult> GetMenuItemsByShop(int shopId)
            => Ok(await _service.GetMenuItemsByShopWithShopNameAsync(shopId));

        // ????????? id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItem(int id)
        {
            var item = await _service.GetMenuItemAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        // ?????????
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItem item)
        {
            if (item.Price < 0)
                return BadRequest("Price must not be negative.");
            var updated = await _service.UpdateMenuItemAsync(id, item);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // ??????
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var deleted = await _service.DeleteMenuItemAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
