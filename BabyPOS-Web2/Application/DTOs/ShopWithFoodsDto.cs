namespace BabyPOS_Web2.Application.DTOs
{
    public class ShopWithFoodsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<MenuItemDto> Foods { get; set; } = new();
    }
}
