namespace SafetyVault.Wpf.Exceptions
{
    public class ItemAlreadyExistsException : Exception
    {
        public ItemAlreadyExistsException(string itemName)
            : base($"Ein Artikel mit dem Namen '{itemName}' existiert bereits.")
        {
        }
    }
}
