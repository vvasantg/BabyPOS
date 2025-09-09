using BabyPOS_Web2.Domain.Models;

namespace BabyPOS_Web2.Application.DTOs
{
    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int ShopId { get; set; }
        public string? ShopName { get; set; }
        public FoodCategory Category { get; set; }
        public string? ImagePath { get; set; }
    }

    public class CreateMenuItemDto
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int ShopId { get; set; }
        public FoodCategory Category { get; set; }
        public string? ImagePath { get; set; }
    }
}
