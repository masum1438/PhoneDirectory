using System.ComponentModel.DataAnnotations;

namespace PhoneDirectoryApi.Models.Dtos
{
    public class CreateContactDto
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        public decimal Balance { get; set; }
        public string Address { get; set; }
        public string Group { get; set; }
    }
}
