
using AutoMapper;
using FluentAssertions;
using OMSAPI.Dtos.ItemDtos;
using OMSAPI.Models;
using OMSAPI.Profiles;
using Xunit;

namespace OMSAPI.UnitTests.Profiles
{
    public class ItemProfileTests
    {
        private readonly IMapper _mapper;

        public ItemProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<ItemProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Should_Map_ItemCreateDto_To_Item()
        {
            var dto = new ItemCreateDto { Name = "Item", UnitOfMeasureCode = "PCS" };

            var result = _mapper.Map<Item>(dto);

            result.Name.Should().Be(dto.Name);
            result.UnitOfMeasureCode.Should().Be(dto.UnitOfMeasureCode);
        }

        [Fact]
        public void Should_Map_Item_To_ItemReadDto()
        {
            var item = new Item { Name = "Item", UnitOfMeasureCode = "PCS" };

            var result = _mapper.Map<ItemReadDto>(item);

            result.Name.Should().Be(item.Name);
        }

        [Fact]
        public void Should_Map_ItemUpdateDto_To_Item()
        {
            var dto = new ItemUpdateDto { Name = "Updated", UnitOfMeasureCode = "PCS" };

            var result = _mapper.Map<Item>(dto);

            result.Name.Should().Be(dto.Name);
        }
    }
}
