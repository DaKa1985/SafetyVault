namespace SafetyVault.Wpf.ViewModels
{
    public class MainViewModel
    {
        public ChecklistViewModel Checklists { get; }
        public ItemCatalogViewModel ItemCatalog { get; }

        public MainViewModel(ChecklistViewModel checklists, ItemCatalogViewModel itemCatalog)
        {
            Checklists = checklists ?? throw new ArgumentNullException(nameof(checklists));
            ItemCatalog = itemCatalog ?? throw new ArgumentNullException(nameof(itemCatalog));
        }
    }
}