using Microsoft.AspNetCore.Identity;

namespace PhoneDirectoryApi.Models.Dtos
{
    public class LoginResult
    {
        public bool Succeeded { get; set; }
        public string Token { get; set; }  // JWT token here
        public IEnumerable<IdentityError> Errors { get; set; }
    }
}
