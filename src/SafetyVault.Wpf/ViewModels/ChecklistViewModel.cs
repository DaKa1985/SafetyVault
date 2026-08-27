using SafetyVault.Wpf.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SafetyVault.Wpf.ViewModels
{
    public class ChecklistViewModel
    {
        public ObservableCollection<ChecklistItem> Items { get; } = new ObservableCollection<ChecklistItem>();
        public ChecklistViewModel()
        {
            Items.Add(new ChecklistItem { Name = "Taschenlampe", Description = "Beleuchtung, auch Stirnlampe denkbar", Quantity = 1, IsPacked = false });
            Items.Add(new ChecklistItem { Name = "Batterien", Description = "Für Taschenlampe, Radio, etc.", Quantity = 4, IsPacked = false });
            Items.Add(new ChecklistItem { Name = "Erste-Hilfe-Set", Description = "Verbandsmaterial, Desinfektionsmittel, Schmerzmittel", Quantity = 1, IsPacked = false });
        }
    }
}
