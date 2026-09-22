using SafetyVault.Wpf.Common;

namespace SafetyVault.Wpf.Models
{
    public class ChecklistItem : NotifyPropertyChangedBase
    {
        private string _name = string.Empty;
        private string? _description;
        private int _targetQuantity;
        private int _actualQuantity;
        private DateTime? _packedAt;
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
        public int TargetQuantity
        {
            get => _targetQuantity;
            set
            {
                if (_targetQuantity == value) return;
                _targetQuantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPacked));
            }
        }
        public int ActualQuantity
        {
            get => _actualQuantity;
            set
            {
                if (_actualQuantity == value) return;
                _actualQuantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsPacked));
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

        public bool IsPacked => ActualQuantity >= TargetQuantity;
    }
}
