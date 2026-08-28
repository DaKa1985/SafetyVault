using SafetyVault.Wpf.Common;
using SafetyVault.Wpf.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SafetyVault.Wpf.ViewModels
{
    public class ChecklistViewModel : NotifyPropertyChangedBase
    {
        private string _newItemName = string.Empty;

        public ChecklistViewModel()
        {
            Items.Add(new ChecklistItem { Name = "Taschenlampe", Description = "Beleuchtung, auch Stirnlampe denkbar", Quantity = 1, IsPacked = false });
            Items.Add(new ChecklistItem { Name = "Batterien", Description = "Für Taschenlampe, Radio, etc.", Quantity = 4, IsPacked = false });
            Items.Add(new ChecklistItem { Name = "Erste-Hilfe-Set", Description = "Verbandsmaterial, Desinfektionsmittel, Schmerzmittel", Quantity = 1, IsPacked = false });
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);
        }
        public string NewItemName
        {
            get => _newItemName;
            set
            {
                if (_newItemName == value) return;
                _newItemName = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ChecklistItem> Items { get; } = new();
        public ICommand AddItemCommand { get; }
        private void AddItem()
        {
            Items.Add(new ChecklistItem { Name = NewItemName });
            NewItemName = string.Empty;
        }
        private bool CanAddItem()
        {
            return !string.IsNullOrWhiteSpace(NewItemName);
        }
    }
}
