using System;

namespace BabyPOS_Web.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsClosed { get; set; }
    }
}
