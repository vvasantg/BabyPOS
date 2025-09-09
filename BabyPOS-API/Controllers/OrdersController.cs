using BabyPOS_API.Application.Services;
using BabyPOS_API.Models;
using Microsoft.AspNetCore.Mvc;
using BabyPOS_API.Data;
using Microsoft.EntityFrameworkCore;

namespace BabyPOS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly AppDbContext _context;
        
        public OrdersController(IOrderService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                // Create order record
                var order = new Order
                {
                    ShopId = request.ShopId,
                    TableId = request.TableId > 0 ? request.TableId : null,
                    CreatedAt = DateTime.UtcNow,
                    IsClosed = false,
                    Status = "Pending",
                    ServiceType = request.ServiceType ?? "dineIn"
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Create order items
                foreach (var item in request.Items)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        MenuItemId = item.MenuItemId,
                        Quantity = item.Quantity
                    };
                    _context.OrderItems.Add(orderItem);
                }

                await _context.SaveChangesAsync();
                return Ok(new { OrderId = order.Id, Message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("table/{tableId}")]
        public async Task<IActionResult> GetOrdersByTable(int tableId)
            => Ok(await _service.GetOrdersByTableAsync(tableId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _service.GetOrderAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPut("{id}/close")]
        public async Task<IActionResult> CloseOrder(int id)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null) return NotFound();
                
                order.IsClosed = true;
                order.CheckedOutAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                
                return Ok(new { Message = "Order closed successfully", CheckedOutAt = order.CheckedOutAt });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusRequest request)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null) return NotFound();
                
                order.Status = request.Status;
                if (request.Status == "Completed")
                {
                    order.IsClosed = true;
                    order.CheckedOutAt = DateTime.UtcNow;
                }
                
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Order status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet("shop/{shopId}")]
        public async Task<IActionResult> GetOrdersByShop(int shopId)
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.Table)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .Where(o => o.ShopId == shopId)
                    .OrderBy(o => o.CreatedAt)
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _service.DeleteOrderAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }

    public class CreateOrderRequest
    {
        public int ShopId { get; set; }
        public int TableId { get; set; } // 0 means no table (takeaway)
        public string? ServiceType { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }

    public class CreateOrderItemRequest
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        public string Status { get; set; } = string.Empty;
    }
}
