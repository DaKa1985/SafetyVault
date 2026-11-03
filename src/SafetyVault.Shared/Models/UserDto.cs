using System;
using System.Collections.Generic;
using System.Text;

namespace SafetyVault.Shared.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
    }
}
