namespace PhoneDirectoryApi.Models.Domain
{
    public class AutoDeleteSettings
    {
        public int Id { get; set; } = 1;
        public bool IsEnabled { get; set; }
        public int DeleteIntervalMinutes { get; set; } = 5;
        public int ContactsToDelete { get; set; } = 10;
        public bool DeleteOnlyInactive { get; set; } = false; 
    }
}
