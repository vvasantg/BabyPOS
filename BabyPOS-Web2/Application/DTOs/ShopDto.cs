namespace BabyPOS_Web2.Application.DTOs
{
    public class ShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public string? OwnerName { get; set; }
    }

    public class CreateShopDto
    {
        public string Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }
    }
}
