namespace SafetyVault.Wpf.ViewModels
{
    public class MainViewModel
    {
        public ChecklistViewModel Checklists { get; }
        public ItemCatalogViewModel ItemCatalog { get; }
        public StockViewModel Stock { get; }

        public MainViewModel(ChecklistViewModel checklists, ItemCatalogViewModel itemCatalog, StockViewModel stock)
        {
            Checklists = checklists ?? throw new ArgumentNullException(nameof(checklists));
            ItemCatalog = itemCatalog ?? throw new ArgumentNullException(nameof(itemCatalog));
            Stock = stock ?? throw new ArgumentNullException(nameof(stock));
        }
    }
}