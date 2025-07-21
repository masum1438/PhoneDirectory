using Microsoft.AspNetCore.Identity;

namespace PhoneDirectoryApi.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {

        // public string FullName { get; set; }
        // public string Email { get; set; }
        // public string Password { get; set; }
        //public string Role { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
    }
}

