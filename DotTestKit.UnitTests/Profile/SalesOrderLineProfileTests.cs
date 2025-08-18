
using AutoMapper;
using FluentAssertions;
using OMSAPI.Dtos.SalesOrderLineDtos;
using OMSAPI.Models;
using OMSAPI.Profiles;
using Xunit;

namespace OMSAPI.UnitTests.Profiles
{
    public class SalesOrderLineProfileTests
    {
        private readonly IMapper _mapper;

        public SalesOrderLineProfileTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SalesOrderLineProfile>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_CreateDto_To_Model()
        {
            var dto = new SalesOrderLineCreateDto
            {
                Quantity = 10,
                Amount = 100,
                ItemId = 1,
                SalesOrderHeaderId = 1
            };

            var model = _mapper.Map<SalesOrderLine>(dto);

            model.Quantity.Should().Be(dto.Quantity);
            model.Amount.Should().Be(dto.Amount);
            model.ItemId.Should().Be(dto.ItemId);
        }

        [Fact]
        public void Should_Map_Model_To_ReadFullDto()
        {
            var model = new SalesOrderLine
            {
                Id = 1,
                Quantity = 20,
                Amount = 300,
                ItemId = 1,
                SalesOrderHeaderId = 2
            };

            var dto = _mapper.Map<SalesOrderLineReadFullDto>(model);

            dto.Id.Should().Be(model.Id);
            dto.Quantity.Should().Be(model.Quantity);
            dto.Amount.Should().Be(model.Amount);
            dto.ItemId.Should().Be(model.ItemId);
        }
    }
}
