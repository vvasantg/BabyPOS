namespace BabyPOS_Web2.Domain.Models
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
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
        public FoodCategory Category { get; set; } = FoodCategory.MainDish;
        public string? ImagePath { get; set; }
    }
}
