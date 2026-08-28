using SafetyVault.Wpf.Common;
using System.Collections.ObjectModel;

namespace SafetyVault.Wpf.Models
{
    public class Checklist : NotifyPropertyChangedBase
    {
        private string _name = string.Empty;
        private string? _description;
        public int Id { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                OnPropertyChanged();
            }
        }
        public string? Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ChecklistItem> Items { get; } = new();
    }
}