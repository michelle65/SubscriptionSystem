using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SubscriptionSystemBackend.Models
{
    [Table("Invitations")]
    public class Invitation
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string InvitationToken { get; set; } = string.Empty;
        
        public bool IsUsed { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UsedAt { get; set; }
    }
}