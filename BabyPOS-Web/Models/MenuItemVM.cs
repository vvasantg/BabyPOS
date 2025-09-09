using System.Collections.Generic;

namespace BabyPOS_Web.Models
{
    public class MenuItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}
