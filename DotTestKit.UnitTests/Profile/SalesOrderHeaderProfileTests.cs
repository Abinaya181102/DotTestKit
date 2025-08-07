//using AutoFixture;
//using AutoFixture.Kernel;
//using AutoMapper;
//using FluentAssertions;
//using OMSAPI.Dtos.SalesOrderHeaderDtos;
//using OMSAPI.Models;
//using OMSAPI.Profiles;
//using Xunit;

//namespace OMSAPI.UnitTests.Profiles
//{
//    public class SalesOrderHeaderProfileTests
//    {
//        private readonly IMapper _mapper;
//        private readonly Fixture _fixture;

//        public SalesOrderHeaderProfileTests()
//        {
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile<SalesOrderHeaderProfile>();

//                // Must ignore unmapped entity members when mapping from DTO to entity
//                cfg.CreateMap<SalesOrderHeaderUpdateDto, SalesOrderHeader>()
//                   .ForMember(dest => dest.Id, opt => opt.Ignore())
//                   .ForMember(dest => dest.Customer, opt => opt.Ignore())
//                   .ForMember(dest => dest.Address, opt => opt.Ignore())
//                   .ForMember(dest => dest.Lines, opt => opt.Ignore());

//                cfg.CreateMap<SalesOrderHeaderCreateDto, SalesOrderHeader>()
//                   .ForMember(dest => dest.Id, opt => opt.Ignore())
//                   .ForMember(dest => dest.Customer, opt => opt.Ignore())
//                   .ForMember(dest => dest.Address, opt => opt.Ignore())
//                   .ForMember(dest => dest.Lines, opt => opt.Ignore());
//            });

//            config.AssertConfigurationIsValid();
//            _mapper = config.CreateMapper();

//            _fixture = new Fixture();
//            // prevent recursion from navigation properties
//            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//        }

//        [Fact]
//        public void Should_Map_SalesOrderHeader_To_SalesOrderHeaderReadDto()
//        {
//            var entity = _fixture.Create<SalesOrderHeader>();

//            var dto = _mapper.Map<SalesOrderHeaderReadDto>(entity);

//            dto.Should().NotBeNull();
//            dto.Id.Should().Be(entity.Id);
//            dto.OrderDate.Should().Be(entity.OrderDate);
//        }

//        [Fact]
//        public void Should_Map_SalesOrderHeader_To_SalesOrderHeaderReadFullDto()
//        {
//            var entity = _fixture.Create<SalesOrderHeader>();

//            var dto = _mapper.Map<SalesOrderHeaderReadFullDto>(entity);

//            dto.Should().NotBeNull();
//            dto.Id.Should().Be(entity.Id);
//            dto.OrderDate.Should().Be(entity.OrderDate);
//            dto.ShipmentDate.Should().Be(entity.ShipmentDate);
//            dto.Profit.Should().Be(entity.Profit);
//        }

//        [Fact]
//        public void Should_Map_SalesOrderHeader_To_SalesOrderHeaderUpdateDto()
//        {
//            var entity = _fixture.Create<SalesOrderHeader>();

//            var dto = _mapper.Map<SalesOrderHeaderUpdateDto>(entity);

//            dto.Should().NotBeNull();
//            dto.OrderDate.Should().Be(entity.OrderDate);
//            dto.ShipmentDate.Should().Be(entity.ShipmentDate);
//            dto.Profit.Should().Be(entity.Profit);
//            dto.CustomerId.Should().Be(entity.CustomerId);
//            dto.AddressId.Should().Be(entity.AddressId);
//        }

//        [Fact]
//        public void Should_Map_SalesOrderHeaderUpdateDto_To_SalesOrderHeader()
//        {
//            var dto = _fixture.Create<SalesOrderHeaderUpdateDto>();

//            var entity = _mapper.Map<SalesOrderHeader>(dto);

//            entity.Should().NotBeNull();
//            entity.OrderDate.Should().Be(dto.OrderDate);
//            entity.ShipmentDate.Should().Be(dto.ShipmentDate);
//            entity.Profit.Should().Be(dto.Profit);
//            entity.CustomerId.Should().Be(dto.CustomerId);
//            entity.AddressId.Should().Be(dto.AddressId);
//        }

//        [Fact]
//        public void Should_Map_SalesOrderHeaderCreateDto_To_SalesOrderHeader()
//        {
//            var dto = _fixture.Create<SalesOrderHeaderCreateDto>();

//            var entity = _mapper.Map<SalesOrderHeader>(dto);

//            entity.Should().NotBeNull();
//            entity.OrderDate.Should().Be(dto.OrderDate);
//            entity.CustomerId.Should().Be(dto.CustomerId);
//            entity.AddressId.Should().Be(dto.AddressId);
//        }
//    }
//}
