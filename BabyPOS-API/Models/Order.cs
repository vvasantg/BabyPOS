using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyPOS_API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int ShopId { get; set; } // เพิ่ม ShopId สำหรับ reporting ที่ง่ายขึ้น
        public int? TableId { get; set; }
        [ForeignKey("TableId")]
        public Table? Table { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CheckedOutAt { get; set; }
        public bool IsClosed { get; set; } = false;
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Ready, Completed
        public string ServiceType { get; set; } = "dineIn"; // dineIn, takeaway, delivery
        
        // Bill relationship
        public int? BillId { get; set; }
        public Bill? Bill { get; set; }
        
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}