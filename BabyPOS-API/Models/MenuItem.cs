using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyPOS_API.Models
{
    public enum FoodCategory
    {
        MainDish = 0,
        Dessert = 1,
        Drink = 2,
        Other = 3
    }

    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public int ShopId { get; set; }
        [ForeignKey("ShopId")]
        public Shop? Shop { get; set; }

        [Required]
        public FoodCategory Category { get; set; } = FoodCategory.MainDish;

        [MaxLength(256)]
        public string? ImagePath { get; set; }
    }
}