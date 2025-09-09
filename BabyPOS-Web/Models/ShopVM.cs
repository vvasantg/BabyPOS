using System.Collections.Generic;

namespace BabyPOS_Web.Models
{
    public class ShopVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
        public List<MenuItemVM> Foods { get; set; } = new();
    }
}
