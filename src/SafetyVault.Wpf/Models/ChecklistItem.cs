using SafetyVault.Wpf.Common;

namespace SafetyVault.Wpf.Models
{
    public class ChecklistItem : NotifyPropertyChangedBase
    {
        private string _name = string.Empty;
        private string? _description;
        private int _quantity;
        private DateTime? _packedAt;
        private bool _isPacked;

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
        public int Quantity 
        {
            get => _quantity;
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnPropertyChanged();
            }
        }
        public DateTime? PackedAt
        {
            get => _packedAt;
            set
            {
                if (_packedAt == value) return;
                _packedAt = value;
                OnPropertyChanged();
            }
        }
        public int ChecklistId { get; set; }

        public bool IsPacked
        {
            get => _isPacked;
            set
            {
                if (_isPacked == value) return;
                _isPacked = value;
                OnPropertyChanged();
            }
        }      
    }
}
