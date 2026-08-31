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
            DeleteItemCommand = new RelayCommand<ChecklistItem>(DeleteItem, CanDeleteItem);

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
        public ICommand DeleteItemCommand { get; }

        private void AddItem()
        {
            Items.Add(new ChecklistItem { Name = NewItemName });
            NewItemName = string.Empty;
        }
        private bool CanAddItem()
        {
            return !string.IsNullOrWhiteSpace(NewItemName);
        }
        private void DeleteItem(ChecklistItem? item)
        {
            if (item is null) return;
            Items.Remove(item);
        }
        private bool CanDeleteItem(ChecklistItem? item) =>
            item is not null;
    }
}
