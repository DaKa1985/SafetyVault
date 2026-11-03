using System.ComponentModel.DataAnnotations;

namespace SafetyVault.Shared.Models
{
    public class LoginDto
    {
        [Required]
        [MaxLength(254)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [MaxLength(128)]
        public string Password { get; set; } = string.Empty;
    }
}
