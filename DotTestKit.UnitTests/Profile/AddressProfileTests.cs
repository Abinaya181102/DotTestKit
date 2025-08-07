using AutoMapper;
using AutoFixture;
using FluentAssertions;
using OMSAPI.Dtos.AddressDtos;
using OMSAPI.Models;
using OMSAPI.Profiles;
using Xunit;

namespace OMSAPI.UnitTests.Profiles
{
    public class AddressProfileTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public AddressProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AddressProfile()));
            _mapper = config.CreateMapper();
            _fixture = new Fixture();
        }

        [Fact]
        public void Should_Map_AddressCreateDto_To_Address()
        {
            var dto = _fixture.Create<AddressCreateDto>();
            var result = _mapper.Map<Address>(dto);
            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public void Should_Map_Address_To_AddressReadDto()
        {
            var model = _fixture.Create<Address>();
            var result = _mapper.Map<AddressReadDto>(model);
            result.Should().BeEquivalentTo(model, options => options.ExcludingMissingMembers());
        }
    }
}
