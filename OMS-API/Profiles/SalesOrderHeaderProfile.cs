using AutoMapper;
using OMSAPI.Dtos.AddressDtos;
using OMSAPI.Dtos.CustomerDtos;
using OMSAPI.Dtos.SalesOrderHeaderDtos;
using OMSAPI.Dtos.SalesOrderLineDtos;
using OMSAPI.Models;

namespace OMSAPI.Profiles
{
    public class SalesOrderHeaderProfile : Profile
    {
        public SalesOrderHeaderProfile()
        {
            CreateMap<SalesOrderHeader, SalesOrderHeaderReadDto>();
            CreateMap<SalesOrderHeader, SalesOrderHeaderReadFullDto>();
            CreateMap<SalesOrderHeader, SalesOrderHeaderUpdateDto>();
            CreateMap<SalesOrderHeaderUpdateDto, SalesOrderHeader>();
            CreateMap<SalesOrderHeaderCreateDto, SalesOrderHeader>();
        }
    }
}
