using AutoMapper;
using OMSAPI.Dtos.AddressDtos;
using OMSAPI.Dtos.CustomerDtos;
using OMSAPI.Models;

namespace OMSAPI.Profiles
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerReadDto>();
            CreateMap<Customer, CustomerReadFullDto>();
            CreateMap<Customer, CustomerUpdateDto>();
            CreateMap<CustomerUpdateDto, Customer>();
            CreateMap<CustomerCreateDto, Customer>();
            CreateMap<Customer, CustomerCreateDto>();
            CreateMap<Address, AddressReadDto>();
        }
    }
}
