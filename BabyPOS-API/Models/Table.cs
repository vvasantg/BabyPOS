using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyPOS_API.Models
{
    public class Table
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public int ShopId { get; set; }
        [ForeignKey("ShopId")]
        public Shop? Shop { get; set; }
    }
}