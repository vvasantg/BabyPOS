using System.ComponentModel.DataAnnotations;

namespace BabyPOS_API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string? Email { get; set; }
    }
}
