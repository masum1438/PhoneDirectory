namespace PhoneDirectoryApi.Models.Dtos
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; }
        public string Address { get; set; }
        public string Group { get; set; }
        public bool Status { get; set; }
       // public string Role { get; set; }
    }
}
