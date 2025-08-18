using AutoMapper;
using AutoFixture;
using FluentAssertions;
using OMSAPI.Dtos.CustomerDtos;
using OMSAPI.Models;
using OMSAPI.Profiles;
using Xunit;

namespace OMSAPI.UnitTests.Profiles
{
    public class CustomerProfileTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public CustomerProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new CustomerProfile()));
            _mapper = config.CreateMapper();
            _fixture = new Fixture();
        }

        [Fact]
        public void Should_Map_CustomerCreateDto_To_Customer()
        {
            var dto = _fixture.Create<CustomerCreateDto>();
            var result = _mapper.Map<Customer>(dto);
            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public void Should_Map_Customer_To_CustomerReadDto()
        {
            var model = _fixture.Create<Customer>();
            var result = _mapper.Map<CustomerReadDto>(model);
            result.Should().BeEquivalentTo(model, options => options.ExcludingMissingMembers());
        }
    }
}
