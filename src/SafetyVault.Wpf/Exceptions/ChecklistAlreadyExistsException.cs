namespace SafetyVault.Wpf.Exceptions
{
    public class ChecklistAlreadyExistsException : Exception
    {
        public ChecklistAlreadyExistsException(string checklistName)
            : base($"Eine Checkliste mit dem Namen '{checklistName}' existiert bereits.")
        {
        }
    }
}