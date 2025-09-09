namespace BabyPOS_Web2.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int? TableId { get; set; }
        public Table? Table { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CheckedOutAt { get; set; }
        public bool IsClosed { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, InProgress, Ready, Completed
        public string ServiceType { get; set; } = "dineIn"; // dineIn, takeaway, delivery
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
