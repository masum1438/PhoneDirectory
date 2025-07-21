using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneDirectoryApi.FileHandlers;
using PhoneDirectoryApi.Models.Domain;
using PhoneDirectoryApi.Models.Dtos;
using PhoneDirectoryApi.Services;

namespace PhoneDirectoryApi.Controllers
{
    
        [Authorize]
        [ApiController]
        [Route("api/[controller]")]
        public class ContactController : ControllerBase
        {
            private readonly IContactService _contactService;
            private readonly ILogger<ContactController> _logger;
          private readonly IAutoDeleteService _autoDeleteService;


        public ContactController(IContactService contactService,IAutoDeleteService autoDeleteService,
          ILogger<ContactController> logger)
        {
            _contactService = contactService;
            _autoDeleteService = autoDeleteService;
            _logger = logger;
        }

        [HttpGet]
            public async Task<ActionResult<IEnumerable<ContactDto>>> GetAll()
            {
                var contacts = await _contactService.GetAllContactsAsync();
                return Ok(contacts);
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<ContactDto>> Get(int id)
            {
                var contact = await _contactService.GetContactAsync(id);

                if (contact == null)
                {
                    _logger.LogWarning("Contact not found: {Id}", id);
                    return NotFound();
                }

                return Ok(contact);
            }

            [Authorize(Roles = "Admin")]
            [HttpPost]
            public async Task<ActionResult<ContactDto>> Create(CreateContactDto contactDto)
            {
                var contact = await _contactService.CreateContactAsync(contactDto);
                _logger.LogInformation("Contact created: {Id}", contact.Id);
                return CreatedAtAction(nameof(Get), new { id = contact.Id }, contact);
            }

            [Authorize(Roles = "Admin")]
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, CreateContactDto contactDto)
            {
                var contact = await _contactService.UpdateContactAsync(id, contactDto);

                if (contact == null)
                {
                    _logger.LogWarning("Contact not found for update: {Id}", id);
                    return NotFound();
                }

                _logger.LogInformation("Contact updated: {Id}", id);
                return NoContent();
            }

            [Authorize(Roles = "Admin")]
            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                await _contactService.DeleteContactAsync(id);
                _logger.LogInformation("Contact deleted: {Id}", id);
                return NoContent();
            }

            [Authorize(Roles = "Admin")]
            [HttpPatch("{id}/disable")]
            public async Task<IActionResult> Disable(int id)
            {
                await _contactService.ToggleContactStatusAsync(id);
                _logger.LogInformation("Contact disabled: {Id}", id);
                return NoContent();
            }

            [Authorize(Roles = "Admin")]
            [HttpPost("bulk")]
            public async Task<IActionResult> BulkInsert(IEnumerable<CreateContactDto> contactDtos)
            {
                await _contactService.BulkInsertContactsAsync(contactDtos);
                _logger.LogInformation("Bulk insert completed: {Count} contacts", contactDtos.Count());
                return Ok(new { Message = $"Successfully inserted {contactDtos.Count()} contacts" });
            }

        //[Authorize(Roles = "Admin")]
        //[HttpDelete("bulk")]
        //public async Task<IActionResult> BulkDelete([FromBody] IEnumerable<int> ids)
        //{
        //    await _contactService.BulkDeleteContactsAsync(ids);
        //    _logger.LogInformation("Bulk delete completed: {Count} contacts", ids.Count());
        //    return Ok(new { Message = $"Successfully deleted {ids.Count()} contacts" });
        //}

        //[Authorize(Roles = "Admin")]
        //[HttpPatch("bulk/disable")]
        //public async Task<IActionResult> BulkDisable([FromBody] IEnumerable<int> ids)
        //{
        //    await _contactService.BulkDisableContactsAsync(ids);
        //    _logger.LogInformation("Bulk disable completed: {Count} contacts", ids.Count());
        //    return Ok(new { Message = $"Successfully disabled {ids.Count()} contacts" });
        //}

        [Authorize(Roles = "Admin")]
        [HttpDelete("bulk")]
        public async Task<IActionResult> BulkDelete([FromBody] List<int> range)
        {
            if (range.Count != 2 || range[0] > range[1])
                return BadRequest("Invalid range. Provide [startId, endId].");

            var ids = Enumerable.Range(range[0], range[1] - range[0] + 1).ToList();

            await _contactService.BulkDeleteContactsAsync(ids);
            _logger.LogInformation("Bulk delete completed: {Count} contacts", ids.Count);
            return Ok(new { Message = $"Successfully deleted {ids.Count} contacts" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("bulk/disable")]
        public async Task<IActionResult> BulkDisable([FromBody] List<int> range)
        {
            if (range.Count != 2 || range[0] > range[1])
                return BadRequest("Invalid range. Provide [startId, endId].");

            var ids = Enumerable.Range(range[0], range[1] - range[0] + 1).ToList();

            await _contactService.BulkDisableContactsAsync(ids);
            _logger.LogInformation("Bulk disable completed: {Count} contacts", ids.Count);
            return Ok(new { Message = $"Successfully disabled {ids.Count} contacts" });
        }


        [Authorize(Roles = "Admin")]
            [HttpPost("import")]
            public async Task<IActionResult> Import(IFormFile file, [FromServices] FileHandler fileHandler)
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                using var stream = file.OpenReadStream();
                var contacts = fileHandler.ParseExcelFile(stream);
                await _contactService.BulkInsertContactsAsync(contacts);

                _logger.LogInformation("Contacts imported from file: {Count}", contacts.Count());
                return Ok(new { Message = $"Successfully imported {contacts.Count()} contacts" });
            }

            [Authorize(Roles = "Admin")]
            [HttpGet("export")]
            public async Task<IActionResult> Export([FromServices] FileHandler fileHandler)
            {
                var contacts = await _contactService.GetAllContactsAsync();
                var stream = fileHandler.GenerateExcelFile(contacts);

                _logger.LogInformation("Contacts exported to file: {Count}", contacts.Count());
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Contacts.xlsx");
            }
        //auto bulk


        [Authorize(Roles = "Admin")]
        [HttpPost("bulk-delete-auto")]
        public async Task<IActionResult> TriggerAutoBulkDelete([FromBody] AutoDeleteRequest request)
        {
            var settings = await _autoDeleteService.GetSettingsAsync();

            if (!settings.IsEnabled)
                return BadRequest("Auto-delete is disabled.");

            int deleteCount = request.ContactsToDelete ?? settings.ContactsToDelete;

            var deletableIds = await _contactService.GetNextDeletableContactIdsAsync(deleteCount);

            if (!deletableIds.Any())
                return Ok("No contacts available for auto-deletion.");

            await _contactService.BulkDeleteContactsAsync(deletableIds);

            _logger.LogInformation("API-triggered auto-delete deleted {Count} contacts", deletableIds.Count);
            return Ok(new { Message = $"Deleted {deletableIds.Count} contacts successfully." });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("toggle/{isEnabled}")]
        public async Task<IActionResult> ToggleAutoDelete(bool isEnabled)
        {
            await _autoDeleteService.ToggleAutoDeleteAsync(isEnabled);
            return Ok(new
            {
                Message = $"Auto-delete has been {(isEnabled ? "enabled" : "disabled")}",
                IsEnabled = isEnabled
            });
        }


    }
}

