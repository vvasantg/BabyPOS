namespace BabyPOS_Web2.Application.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int? TableId { get; set; }
        public string? TableName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? CheckedOutAt { get; set; }
        public bool IsClosed { get; set; }
        public string Status { get; set; } = "Pending";
        public string ServiceType { get; set; } = "dineIn";
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public decimal TotalAmount => OrderItems.Sum(item => item.Price * item.Quantity);
    }

    public class CreateOrderDto
    {
        public int? TableId { get; set; }
        public string ServiceType { get; set; } = "dineIn";
        public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}
