using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using PhoneDirectoryApi.Data;
using PhoneDirectoryApi.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneDirectoryApi.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly PhoneDirectoryContext _context;

        public ContactRepository(PhoneDirectoryContext context)
        {
            _context = context;
        }

        public async Task<Contact> GetByIdAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task AddAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Contact contact)
        {
            contact.UpdatedAt = DateTime.UtcNow;
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contact = await GetByIdAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task DisableAsync(int id)
        //{
        //    var contact = await GetByIdAsync(id);
        //    if (contact != null)
        //    {
        //        contact.Status = false;  // Assuming Status is a bool indicating active/inactive
        //        await UpdateAsync(contact);
        //    }
        //}
        public async Task ToggleContactStatusAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
                throw new KeyNotFoundException($"Contact with id {id} not found.");

            // Toggle status (assuming bool IsActive)
            contact.Status = !contact.Status;

            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }


        public async Task BulkInsertAsync(IEnumerable<Contact> contacts)
        {
            await _context.Contacts.AddRangeAsync(contacts);
            await _context.SaveChangesAsync();
        }

        public async Task BulkDeleteAsync(IEnumerable<int> ids)
        {
            var contacts = await _context.Contacts
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            _context.Contacts.RemoveRange(contacts);
            await _context.SaveChangesAsync();
        }

        public async Task BulkDisableAsync(IEnumerable<int> ids)
        {
            var contacts = await _context.Contacts
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            foreach (var contact in contacts)
            {
                contact.Status = false;
                contact.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }
        public async Task<List<int>> GetNextDeletableContactIdsAsync(int count, bool onlyInactive = false)
        {
            var query = _context.Contacts.AsQueryable();

            if (onlyInactive)
            {
                query = query.Where(c => !c.Status);
            }

            return await query
                .OrderBy(c => c.CreatedAt) // Delete oldest first
                .Take(count)
                .Select(c => c.Id)
                .ToListAsync();
        }

        public async Task<int> BulkDeleteContactsAsync(IEnumerable<int> ids)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var contacts = await _context.Contacts
                    .Where(c => ids.Contains(c.Id))
                    .ToListAsync();

                if (!contacts.Any())
                {
                    return 0;
                }

                _context.Contacts.RemoveRange(contacts);
                var result = await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return result;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }



    }
}
