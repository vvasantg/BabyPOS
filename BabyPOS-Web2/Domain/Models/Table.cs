namespace BabyPOS_Web2.Domain.Models
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ShopId { get; set; }
        public Shop? Shop { get; set; }
    }
}
