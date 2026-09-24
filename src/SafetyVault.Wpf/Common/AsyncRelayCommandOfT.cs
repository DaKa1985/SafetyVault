using System.Windows.Input;

namespace SafetyVault.Wpf.Common
{
    public class AsyncRelayCommand<T> : ICommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool>? _canExecute;

        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter is T param ? param : default!) ?? true;
        }

        public async void Execute(object? parameter)
        {
            if (parameter is T param)
            {
                try
                {
                    await _execute(param);
                }
                finally
                {
                    CommandManager.InvalidateRequerySuggested();
                }
            }
        }
    }
}
