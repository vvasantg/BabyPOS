namespace BabyPOS_Web2.Application.DTOs;

public class CartItemDto
{
    public int MenuItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImagePath { get; set; } = "/img/food/default.png";
}
