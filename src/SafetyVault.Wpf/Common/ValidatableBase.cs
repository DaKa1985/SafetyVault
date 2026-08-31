using System.Collections;
using System.ComponentModel;

namespace SafetyVault.Wpf.Common
{
    public abstract class ValidatableBase : NotifyPropertyChangedBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _errors = new();
        
        public bool HasErrors => _errors.Count > 0;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName) && _errors.TryGetValue(propertyName, out var errors))
            {
                return errors;
            }
            return Array.Empty<string>();
        }
        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            OnPropertyChanged(nameof(HasErrors));
        }
        protected void SetErrors(string propertyName, IEnumerable<string> errors)
        {
            if (errors.Any())
            {
                _errors[propertyName] = new List<string>(errors);
            }
            else
            {
                _errors.Remove(propertyName);
            }
            OnErrorsChanged(propertyName);
        }
        protected void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName))
            {
                OnErrorsChanged(propertyName);
            }
        }
    }
}
