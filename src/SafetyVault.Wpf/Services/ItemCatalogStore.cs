using SafetyVault.Shared.Models;
using System.Collections.ObjectModel;

namespace SafetyVault.Wpf.Services
{
    public class ItemCatalogStore : IItemCatalogStore
    {
        private readonly IItemApiClient _itemApiClient;
        private readonly ObservableCollection<ItemDto> _items;
        public ReadOnlyObservableCollection<ItemDto> Items { get; }
        public ItemCatalogStore(IItemApiClient itemApiClient)
        {
            _itemApiClient = itemApiClient ?? throw new ArgumentNullException(nameof(itemApiClient));
            _items = new ObservableCollection<ItemDto>();
            Items = new ReadOnlyObservableCollection<ItemDto>(_items);
        }
        public async Task LoadAsync()
        {
            var items = await _itemApiClient.GetItemsAsync();
            _items.Clear();
            foreach (var item in items)
            {
                _items.Add(item);
            }
        }
    }
}
