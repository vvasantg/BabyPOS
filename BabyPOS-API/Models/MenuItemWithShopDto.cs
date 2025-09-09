using System.Collections.Generic;

namespace BabyPOS_API.Models
{
    public class MenuItemWithShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int ShopId { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public int Category { get; set; }
        public string? ImagePath { get; set; }
    }
}
