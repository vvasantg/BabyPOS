namespace BabyPOS_Web2.Application.DTOs;

public class TableDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int ShopId { get; set; }
}

public class CreateTableDto
{
    public string Name { get; set; } = string.Empty;
    public int ShopId { get; set; }
}
