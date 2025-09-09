using System.ComponentModel.DataAnnotations;

namespace BabyPOS_API.Models
{
    public class Bill
    {
        public int Id { get; set; }
        
        [Required]
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
        
        public int? TableId { get; set; }
        public Table? Table { get; set; }
        
        [Required]
        public string ServiceType { get; set; } = "dineIn"; // dineIn, takeaway, delivery
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
        
        public bool IsPaid { get; set; } = false;
        
        public decimal TotalAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal FinalAmount => TotalAmount - (DiscountAmount ?? 0);
        
        // For delivery
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal? DeliveryFee { get; set; }
        
        // Bill items (collected from multiple orders)
        public List<Order> Orders { get; set; } = new List<Order>();
        
        public string? Notes { get; set; }
        
        // Billing logic
        public bool CanCombineOrders => ServiceType == "dineIn" || ServiceType == "takeaway";
        public bool MustSeparateBills => ServiceType == "delivery";
    }
}
