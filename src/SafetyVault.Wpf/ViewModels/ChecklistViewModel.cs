using SafetyVault.Wpf.Common;
using SafetyVault.Wpf.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SafetyVault.Wpf.ViewModels
{
    public class ChecklistViewModel : ValidatableBase
    {
        private string _newItemName = string.Empty;
        private string _newItemQuantityText = "1";
        private int _newItemQuantity;

        public ChecklistViewModel()
        {
            Items.Add(new ChecklistItem { Name = "Taschenlampe", Description = "Beleuchtung, auch Stirnlampe denkbar", Quantity = 1, IsPacked = false });
            Items.Add(new ChecklistItem { Name = "Batterien", Description = "Für Taschenlampe, Radio, etc.", Quantity = 4, IsPacked = false });
            Items.Add(new ChecklistItem { Name = "Erste-Hilfe-Set", Description = "Verbandsmaterial, Desinfektionsmittel, Schmerzmittel", Quantity = 1, IsPacked = false });
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);
            DeleteItemCommand = new RelayCommand<ChecklistItem>(DeleteItem, CanDeleteItem);
            ValidateNewItemName();
            ValidateNewItemQuantityText();

        }
        public string NewItemName
        {
            get => _newItemName;
            set
            {
                if (_newItemName == value) return;
                _newItemName = value;
                OnPropertyChanged();
                ValidateNewItemName();
            }
        }

        public string NewItemQuantityText
        {
            get => _newItemQuantityText;
            set
            {
                if (_newItemQuantityText == value) return;
                _newItemQuantityText = value;
                OnPropertyChanged();
                ValidateNewItemQuantityText();
            }
        }

        public ObservableCollection<ChecklistItem> Items { get; } = new();
        public ICommand AddItemCommand { get; }
        public ICommand DeleteItemCommand { get; }

        private void AddItem()
        {
            Items.Add(new ChecklistItem { Name = NewItemName, Quantity = _newItemQuantity });
            NewItemName = string.Empty;
            NewItemQuantityText = "1";

        }
        private bool CanAddItem() =>
            !HasErrors;
        private void DeleteItem(ChecklistItem? item)
        {
            if (item is null) return;
            Items.Remove(item);
        }
        private bool CanDeleteItem(ChecklistItem? item) =>
            item is not null;
        private void ValidateNewItemName()
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(NewItemName))
            {
                errors.Add("Der Name darf nicht leer sein.");
            }
            if (NewItemName.Length > 100)
            {
                errors.Add("Der Name darf nicht länger als 100 Zeichen sein.");
            }
            SetErrors(nameof(NewItemName), errors);
        }
        private void ValidateNewItemQuantityText()
        {
            var errors = new List<string>();
            if (!int.TryParse(NewItemQuantityText, out var quantity) || quantity <= 0)
            {
                errors.Add("Die Menge muss eine positive Zahl sein.");
            }
            else if (quantity > 1000)
            {
                errors.Add("Die Menge darf nicht größer als 1000 sein.");
            }
            else
            {
                _newItemQuantity = quantity;
            }
            SetErrors(nameof(NewItemQuantityText), errors);
        }
    }
}
