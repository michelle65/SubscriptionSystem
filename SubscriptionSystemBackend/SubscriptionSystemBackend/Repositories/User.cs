using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionSystemBackend.Repositories
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;
        
        public bool IsConfirmed { get; set; }
        
        [MaxLength(16)]
        public string FiscalCode { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}