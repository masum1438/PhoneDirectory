namespace PhoneDirectoryApi.Models.Domain
{
    public class AutoDeleteRequest
    {
        public int? ContactsToDelete { get; set; }
        public int? IntervalMinutes { get; set; }
    }

}
