using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BabyPOS_API.Data;
using BabyPOS_API.Models;

namespace BabyPOS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BillsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/bills/shop/{shopId}
        [HttpGet("shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetBillsByShop(int shopId)
        {
            var bills = await _context.Bills
                .Where(b => b.ShopId == shopId)
                .Include(b => b.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                .Include(b => b.Table)
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new
                {
                    b.Id,
                    b.ShopId,
                    b.TableId,
                    TableName = b.Table != null ? b.Table.Name : null,
                    b.ServiceType,
                    b.CreatedAt,
                    b.PaidAt,
                    b.IsPaid,
                    b.TotalAmount,
                    b.FinalAmount,
                    b.CustomerName,
                    b.CustomerPhone,
                    b.DeliveryAddress,
                    b.DeliveryFee,
                    Orders = b.Orders.Select(o => new
                    {
                        o.Id,
                        o.Status,
                        o.CreatedAt,
                        OrderItems = o.OrderItems.Select(oi => new
                        {
                            oi.Id,
                            MenuItemName = oi.MenuItem.Name,
                            oi.Quantity,
                            oi.Price
                        })
                    })
                })
                .ToListAsync();

            return Ok(bills);
        }

        // POST: api/bills/generate/{shopId}
        [HttpPost("generate/{shopId}")]
        public async Task<ActionResult<object>> GenerateBills(int shopId)
        {
            try
            {
                var pendingOrders = await _context.Orders
                    .Where(o => o.ShopId == shopId && o.Status == "Ready" && o.BillId == null)
                    .Include(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                    .Include(o => o.Table)
                    .ToListAsync();

                if (!pendingOrders.Any())
                {
                    return Ok(new { message = "ไม่มีออเดอร์ที่พร้อมออกบิล" });
                }

                var billsCreated = new List<Bill>();

                // Group orders for billing based on service type
                var orderGroups = pendingOrders.GroupBy(o => new
                {
                    o.ServiceType,
                    o.TableId,
                    // For delivery, each order gets its own bill
                    DeliveryGroupKey = o.ServiceType == "delivery" ? o.Id : (int?)null
                });

                foreach (var group in orderGroups)
                {
                    var bill = new Bill
                    {
                        ShopId = shopId,
                        TableId = group.Key.TableId,
                        ServiceType = group.Key.ServiceType,
                        CreatedAt = DateTime.UtcNow,
                        TotalAmount = group.Sum(o => o.OrderItems.Sum(oi => oi.Quantity * oi.Price)),
                        Orders = group.ToList()
                    };

                    _context.Bills.Add(bill);
                    billsCreated.Add(bill);

                    // Update orders with bill reference
                    foreach (var order in group)
                    {
                        order.BillId = bill.Id;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new 
                { 
                    message = $"สร้างบิลสำเร็จ {billsCreated.Count} บิล",
                    billsCount = billsCreated.Count,
                    ordersProcessed = pendingOrders.Count
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาดในการสร้างบิล", error = ex.Message });
            }
        }

        // PUT: api/bills/{id}/pay
        [HttpPut("{id}/pay")]
        public async Task<ActionResult<object>> PayBill(int id)
        {
            try
            {
                var bill = await _context.Bills
                    .Include(b => b.Orders)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (bill == null)
                {
                    return NotFound(new { message = "ไม่พบบิลที่ระบุ" });
                }

                if (bill.IsPaid)
                {
                    return BadRequest(new { message = "บิลนี้ชำระเงินแล้ว" });
                }

                bill.IsPaid = true;
                bill.PaidAt = DateTime.UtcNow;

                // Update all orders in this bill to Completed
                foreach (var order in bill.Orders)
                {
                    order.Status = "Completed";
                    order.CheckedOutAt = DateTime.UtcNow;
                    order.IsClosed = true;
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "ชำระเงินสำเร็จ" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "เกิดข้อผิดพลาดในการชำระเงิน", error = ex.Message });
            }
        }

        // GET: api/bills/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetBill(int id)
        {
            var bill = await _context.Bills
                .Include(b => b.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.MenuItem)
                .Include(b => b.Table)
                .Include(b => b.Shop)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bill == null)
            {
                return NotFound(new { message = "ไม่พบบิลที่ระบุ" });
            }

            return Ok(new
            {
                bill.Id,
                bill.ShopId,
                ShopName = bill.Shop.Name,
                bill.TableId,
                TableName = bill.Table?.Name,
                bill.ServiceType,
                bill.CreatedAt,
                bill.PaidAt,
                bill.IsPaid,
                bill.TotalAmount,
                bill.FinalAmount,
                bill.CustomerName,
                bill.CustomerPhone,
                bill.DeliveryAddress,
                bill.DeliveryFee,
                Orders = bill.Orders.Select(o => new
                {
                    o.Id,
                    o.Status,
                    o.CreatedAt,
                    OrderItems = o.OrderItems.Select(oi => new
                    {
                        oi.Id,
                        MenuItemName = oi.MenuItem.Name,
                        oi.Quantity,
                        oi.Price,
                        Total = oi.Quantity * oi.Price
                    })
                })
            });
        }
    }
}
