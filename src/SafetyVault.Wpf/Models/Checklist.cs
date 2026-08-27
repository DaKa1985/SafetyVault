using System;
using System.Collections.Generic;
using System.Text;

namespace SafetyVault.Wpf.Models
{
    public class Checklist
    { 
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<ChecklistItem> Items { get; set; } = new List<ChecklistItem>();

    }
}
