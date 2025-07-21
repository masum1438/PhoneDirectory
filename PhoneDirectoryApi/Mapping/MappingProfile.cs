using AutoMapper;
using PhoneDirectoryApi.Models.Domain;
using PhoneDirectoryApi.Models.Dtos;

namespace PhoneDirectoryApi.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Contact, ContactDto>();
            CreateMap<CreateContactDto, Contact>();
       
        }
    }
}
