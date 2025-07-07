using System.ComponentModel.DataAnnotations;

namespace SubscriptionSystemBackend.Models
{
    public class UpdateFiscalCodeDto
    {
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Fiscal code must be exactly 16 characters")]
        public string FiscalCode { get; set; } = string.Empty;
    }
} 