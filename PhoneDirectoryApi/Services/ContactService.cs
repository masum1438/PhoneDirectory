using AutoMapper;
using PhoneDirectoryApi.Models.Domain;
using PhoneDirectoryApi.Models.Dtos;
using PhoneDirectoryApi.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneDirectoryApi.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _repository;
        private readonly IMapper _mapper;

        public ContactService(IContactRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ContactDto> GetContactAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            return _mapper.Map<ContactDto>(contact);
        }

        public async Task<IEnumerable<ContactDto>> GetAllContactsAsync()
        {
            var contacts = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ContactDto>>(contacts);
        }

        public async Task<ContactDto> CreateContactAsync(CreateContactDto contactDto)
        {
            var contact = _mapper.Map<Contact>(contactDto);
            await _repository.AddAsync(contact);
            return _mapper.Map<ContactDto>(contact);
        }

        public async Task<ContactDto> UpdateContactAsync(int id, CreateContactDto contactDto)
        {
            var existingContact = await _repository.GetByIdAsync(id);
            if (existingContact == null)
                return null;

            _mapper.Map(contactDto, existingContact);
            await _repository.UpdateAsync(existingContact);
            return _mapper.Map<ContactDto>(existingContact);
        }

        public async Task DeleteContactAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ToggleContactStatusAsync(int id)
        {
            await _repository.ToggleContactStatusAsync(id);
        }

        public async Task BulkInsertContactsAsync(IEnumerable<CreateContactDto> contactDtos)
        {
            var contacts = _mapper.Map<IEnumerable<Contact>>(contactDtos);
            // Assuming repository method performs insert one by one internally
            await _repository.BulkInsertAsync(contacts);
        }

        public async Task BulkDeleteContactsAsync(IEnumerable<int> ids)
        {
            // Assuming repository method performs delete one by one internally
            await _repository.BulkDeleteAsync(ids);
        }

        public async Task BulkDisableContactsAsync(IEnumerable<int> ids)
        {
            // Assuming repository method performs disable one by one internally
            await _repository.BulkDisableAsync(ids);
        }
        public async Task<List<int>> GetNextDeletableContactIdsAsync(int count, bool onlyInactive = false)
        {
            return await _repository.GetNextDeletableContactIdsAsync(count,onlyInactive);
        }
    }
}
