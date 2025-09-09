using System.Collections.Generic;

namespace BabyPOS_API.Models
{
    public class ShopWithFoodsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<MenuItemDto> Foods { get; set; } = new();
    }

    public class MenuItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
    }
}
