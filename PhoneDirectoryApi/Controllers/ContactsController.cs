using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneDirectoryApi.Data;
using PhoneDirectoryApi.Models.Domain;
using PhoneDirectoryApi.Models.Dtos;

namespace PhoneDirectoryApi.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private readonly PhoneDirectoryContext _context;

        public ContactsController(PhoneDirectoryContext context)
        {
            _context = context;
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var contacts = await _context.Contacts.ToListAsync();
            var dtoList = contacts.Select(c => new ContactDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Balance = c.Balance,
                Address = c.Address,
                Group = c.Group,
                Status = c.Status
            }).ToList();

            return View(dtoList);
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null) return NotFound();

            var dto = new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Balance = contact.Balance,
                Address = contact.Address,
                Group = contact.Group,
                Status = contact.Status
            };

            return View(dto);
        }

        // GET: Contacts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ContactDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var contact = new Contact
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Balance = dto.Balance,
                Address = dto.Address,
                Group = dto.Group,
                Status = true
            };

            _context.Add(contact);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Contact created successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Contacts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();

            var dto = new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Balance = contact.Balance,
                Address = contact.Address,
                Group = contact.Group,
                Status = contact.Status
            };

            return View(dto);
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, ContactDto dto)
        {
            if (id != dto.Id) return NotFound();

            if (!ModelState.IsValid)
                return View(dto);

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();

            contact.Name = dto.Name;
            contact.Email = dto.Email;
            contact.PhoneNumber = dto.PhoneNumber;
            contact.Balance = dto.Balance;
            contact.Address = dto.Address;
            contact.Group = dto.Group;
            contact.Status = dto.Status;

            _context.Update(contact);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Contact updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Contacts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var contact = await _context.Contacts.FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null) return NotFound();

            var dto = new ContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                Balance = contact.Balance,
                Address = contact.Address,
                Group = contact.Group,
                Status = contact.Status
            };

            return View(dto);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Contact deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Contacts/Disable/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Disable(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) return NotFound();

            contact.Status = !contact.Status;
            _context.Update(contact);
            await _context.SaveChangesAsync();

            TempData["Message"] = contact.Status ? "Contact enabled!" : "Contact disabled!";
            return RedirectToAction(nameof(Index));
        }
    }
}
