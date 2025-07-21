namespace PhoneDirectoryApi.Models.ViewModels
{
    public class UserWithRolesViewModel
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public List<string> Roles { get; set; }
    }
}
