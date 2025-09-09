using BabyPOS_API.Application.Services;
using BabyPOS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BabyPOS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemService _service;
        public OrderItemsController(IOrderItemService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddOrderItem([FromBody] OrderItem item)
            => Ok(await _service.AddOrderItemAsync(item));

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderItemsByOrder(int orderId)
            => Ok(await _service.GetOrderItemsByOrderAsync(orderId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItem(int id)
        {
            var item = await _service.GetOrderItemAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItem item)
        {
            var updated = await _service.UpdateOrderItemAsync(id, item);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            var deleted = await _service.DeleteOrderItemAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
