namespace BabyPOS_Web2.Application.DTOs
{
    public class BillDto
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public int? TableId { get; set; }
        public string? TableName { get; set; }
        public string ServiceType { get; set; } = "dineIn";
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
        public bool IsPaid { get; set; } = false;
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public string? DeliveryAddress { get; set; }
        public decimal? DeliveryFee { get; set; }
        public List<OrderDto> Orders { get; set; } = new List<OrderDto>();
    }

    public class GenerateBillsDto
    {
        public string Message { get; set; } = string.Empty;
        public int BillsCount { get; set; }
        public int OrdersProcessed { get; set; }
    }
}
