namespace SafetyVault.Wpf.Exceptions
{
    public class InsufficientQuantityException : Exception
    {
        public InsufficientQuantityException(int requestedQuantity)
            : base($"Nicht genügend Bestand vom Artikel um die angeforderte Menge '{requestedQuantity}' zu erfüllen.")
        {
        }
    }
}
