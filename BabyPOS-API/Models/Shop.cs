using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BabyPOS_API.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User? Owner { get; set; }
    }
}