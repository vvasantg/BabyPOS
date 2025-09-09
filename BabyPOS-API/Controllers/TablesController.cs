using BabyPOS_API.Application.Services;
using BabyPOS_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BabyPOS_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TablesController : ControllerBase
    {
        private readonly ITableService _service;
        public TablesController(ITableService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable([FromBody] Table table)
            => Ok(await _service.CreateTableAsync(table));

        [HttpGet("shop/{shopId}")]
        public async Task<IActionResult> GetTablesByShop(int shopId)
            => Ok(await _service.GetTablesByShopAsync(shopId));

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTable(int id)
        {
            var table = await _service.GetTableAsync(id);
            if (table == null) return NotFound();
            return Ok(table);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTable(int id, [FromBody] Table table)
        {
            var updated = await _service.UpdateTableAsync(id, table);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var deleted = await _service.DeleteTableAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
