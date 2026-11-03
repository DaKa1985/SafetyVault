using System.ComponentModel.DataAnnotations;

namespace SafetyVault.Shared.Models
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(254)]
        [EmailAddress]
        public string UserEmail { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }
}
