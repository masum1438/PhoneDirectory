using PhoneDirectoryApi.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneDirectoryApi.Repositories
{
    public interface IContactRepository
    {
        Task<Contact> GetByIdAsync(int id);
        Task<IEnumerable<Contact>> GetAllAsync();
        Task AddAsync(Contact contact);
        Task UpdateAsync(Contact contact);
        Task DeleteAsync(int id);
        Task ToggleContactStatusAsync(int id);
        Task BulkInsertAsync(IEnumerable<Contact> contacts);
        Task BulkDeleteAsync(IEnumerable<int> ids);
        Task BulkDisableAsync(IEnumerable<int> ids);
        //Task<List<int>> GetNextDeletableContactIdsAsync(int count);
        Task<List<int>> GetNextDeletableContactIdsAsync(int count, bool onlyInactive = false);


    }
}
