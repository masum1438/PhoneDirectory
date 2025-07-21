using ClosedXML.Excel;
using PhoneDirectoryApi.Models.Dtos;

namespace PhoneDirectoryApi.FileHandlers
{
    public class FileHandler
    {
        public IEnumerable<CreateContactDto> ParseExcelFile(Stream fileStream)
        {
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet(1);
            var contacts = new List<CreateContactDto>();

            foreach (var row in worksheet.RowsUsed().Skip(1))
            {
                var contact = new CreateContactDto
                {
                    Name = row.Cell(1).GetString(),
                    Email = row.Cell(2).GetString(),
                    PhoneNumber = row.Cell(3).GetString(),
                    Balance = decimal.TryParse(row.Cell(4).GetString(), out var balance)
                    ? balance : 0,

                    Address = row.Cell(5).GetString(),
                    Group = row.Cell(6).GetString()
                };
                contacts.Add(contact);
            }

            return contacts;
        }

        public MemoryStream GenerateExcelFile(IEnumerable<ContactDto> contacts)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Contacts");

            // Add headers
            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Email";
            worksheet.Cell(1, 3).Value = "Phone Number";
            worksheet.Cell(1, 4).Value = "Balance";
            worksheet.Cell(1, 5).Value = "Address";
            worksheet.Cell(1, 6).Value = "Group";
            worksheet.Cell(1, 7).Value = "Status";

            // Add data
            int row = 2;
            foreach (var contact in contacts)
            {
                worksheet.Cell(row, 1).Value = contact.Name;
                worksheet.Cell(row, 2).Value = contact.Email;
                worksheet.Cell(row, 3).Value = contact.PhoneNumber;
                worksheet.Cell(row, 4).Value = contact.Balance;
                worksheet.Cell(row, 5).Value = contact.Address;
                worksheet.Cell(row, 6).Value = contact.Group;
                worksheet.Cell(row, 7).Value = contact.Status;
                row++;
            }

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return stream;
        }
    }
}
