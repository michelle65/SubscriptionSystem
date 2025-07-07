﻿using System.ComponentModel.DataAnnotations;

namespace SubscriptionSystemBackend.Models
{
    public class RegisterAdminDto
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        
        [Required]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        
        public string? FiscalCode { get; set; }
    }
}
