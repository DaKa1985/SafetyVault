using System;
using System.Collections.Generic;
using System.Text;

namespace SafetyVault.Wpf.Models
{
    public class ChecklistItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public bool IsPacked { get; set; }
        public DateTime? PackedAt { get; set; }
        public int ChecklistId { get; set; }
    }
}
