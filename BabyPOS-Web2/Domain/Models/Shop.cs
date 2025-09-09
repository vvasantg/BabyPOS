namespace BabyPOS_Web2.Domain.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
    }
}
