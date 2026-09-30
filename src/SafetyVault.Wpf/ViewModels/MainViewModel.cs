namespace SafetyVault.Wpf.ViewModels
{
    public class MainViewModel
    {
        public ChecklistViewModel Checklists { get; }

        public MainViewModel(ChecklistViewModel checklists)
        {
            Checklists = checklists ?? throw new ArgumentNullException(nameof(checklists));
        }
    }
}
