//using AutoFixture;
//using AutoMapper;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using OMSAPI.Controllers;
//using OMSAPI.Dtos.ItemDtos;
//using OMSAPI.Interfaces;
//using OMSAPI.Models;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace OMSAPI.UnitTests.Controllers
//{
//    public class ItemControllerTests
//    {
//        private readonly Mock<IItem> _mockItemService;
//        private readonly IMapper _mapper;
//        private readonly Fixture _fixture;
//        private readonly ItemController _controller;

//        public ItemControllerTests()
//        {
//            _mockItemService = new Mock<IItem>();
//            _fixture = new Fixture();

//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile(new OMSAPI.Profiles.ItemProfile());
//            });
//            _mapper = config.CreateMapper();

//            _controller = new ItemController(_mockItemService.Object, _mapper);
//        }

//        [Fact]
//        public void GetItem_ReturnsOk_WhenItemExists()
//        {
//            var item = _fixture.Create<Item>();
//            _mockItemService.Setup(s => s.Get(item.Id)).Returns(item);

//            var result = _controller.GetItem(item.Id);

//            result.Result.Should().BeOfType<OkObjectResult>();
//        }

//        [Fact]
//        public void GetItem_ReturnsNotFound_WhenItemDoesNotExist()
//        {
//            _mockItemService.Setup(s => s.Get(It.IsAny<int>())).Returns((Item)null);

//            var result = _controller.GetItem(1);

//            result.Result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void GetAll_ReturnsListOfItems()
//        {
//            var items = _fixture.CreateMany<Item>(3).ToList();
//            _mockItemService.Setup(s => s.GetAll()).Returns(items);

//            var result = _controller.GetAll();

//            result.Result.Should().BeOfType<OkObjectResult>();
//        }

//        [Fact]
//        public void Create_ReturnsCreatedAtRouteResult()
//        {
//            var dto = _fixture.Create<ItemCreateDto>();

//            var result = _controller.Create(dto);

//            result.Should().BeOfType<CreatedAtRouteResult>();
//        }

//        [Fact]
//        public void Delete_ReturnsNoContent_WhenItemExists()
//        {
//            var item = _fixture.Create<Item>();
//            _mockItemService.Setup(s => s.Get(item.Id)).Returns(item);

//            var result = _controller.Delete(item.Id);

//            result.Should().BeOfType<NoContentResult>();
//        }

//        [Fact]
//        public void Delete_ReturnsNotFound_WhenItemNotExists()
//        {
//            _mockItemService.Setup(s => s.Get(It.IsAny<int>())).Returns((Item)null);

//            var result = _controller.Delete(1);

//            result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void Update_ReturnsNoContent_WhenValid()
//        {
//            var item = _fixture.Create<Item>();
//            var updateDto = _fixture.Create<ItemUpdateDto>();
//            _mockItemService.Setup(s => s.Get(item.Id)).Returns(item);

//            var result = _controller.Update(item.Id, updateDto);

//            result.Should().BeOfType<NoContentResult>();
//        }
//    }
//}



using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMSAPI.Controllers;
using OMSAPI.Dtos.ItemDtos;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OMSAPI.UnitTests.Controllers
{
    public class ItemControllerTests
    {
        private readonly Mock<IItem> _mockItemService;
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;
        private readonly ItemController _controller;

        public ItemControllerTests()
        {
            _mockItemService = new Mock<IItem>();
            _fixture = new Fixture();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new OMSAPI.Profiles.ItemProfile());
            });
            _mapper = config.CreateMapper();

            _controller = new ItemController(_mockItemService.Object, _mapper);
        }

        [Fact]
        public void GetItem_ReturnsOk_WhenItemExists()
        {
            var item = _fixture.Create<Item>();
            _mockItemService.Setup(s => s.Get(item.Id)).Returns(item);

            var result = _controller.GetItem(item.Id);

            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)!.Value.Should().BeOfType<ItemReadFullDto>();
        }

        [Fact]
        public void GetItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            var id = _fixture.Create<int>();
            _mockItemService.Setup(s => s.Get(id)).Returns((Item)null!);

            var result = _controller.GetItem(id);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAll_ReturnsOk_WithList()
        {
            var items = _fixture.CreateMany<Item>(3).ToList();
            _mockItemService.Setup(s => s.GetAll()).Returns(items);

            var result = _controller.GetAll();

            result.Result.Should().BeOfType<OkObjectResult>();
            (result.Result as OkObjectResult)!.Value.Should().BeAssignableTo<IEnumerable<ItemReadDto>>();
        }

        [Fact]
        public void GetAll_ReturnsOk_WithEmptyList()
        {
            _mockItemService.Setup(s => s.GetAll()).Returns(new List<Item>());

            var result = _controller.GetAll();

            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeAssignableTo<IEnumerable<ItemReadDto>>();
            ((IEnumerable<ItemReadDto>)okResult.Value).Should().BeEmpty();
        }

        [Fact]
        public void Create_ReturnsCreatedAtRouteResult()
        {
            var createDto = _fixture.Create<ItemCreateDto>();
            var model = _mapper.Map<Item>(createDto);

            _mockItemService.Setup(s => s.Create(It.IsAny<Item>()));
            _mockItemService.Setup(s => s.SaveChanges());

            var result = _controller.Create(createDto);

            result.Should().BeOfType<CreatedAtRouteResult>();
            var created = result as CreatedAtRouteResult;
            created!.RouteName.Should().Be("GetItem");
            created.Value.Should().BeOfType<ItemReadFullDto>();
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenItemExists()
        {
            var item = _fixture.Create<Item>();
            _mockItemService.Setup(s => s.Get(item.Id)).Returns(item);

            var result = _controller.Delete(item.Id);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenItemDoesNotExist()
        {
            var id = _fixture.Create<int>();
            _mockItemService.Setup(s => s.Get(id)).Returns((Item)null!);

            var result = _controller.Delete(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenValid()
        {
            var item = _fixture.Create<Item>();
            var updateDto = _fixture.Create<ItemUpdateDto>();
            _mockItemService.Setup(s => s.Get(item.Id)).Returns(item);

            var result = _controller.Update(item.Id, updateDto);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenItemDoesNotExist()
        {
            var id = _fixture.Create<int>();
            var updateDto = _fixture.Create<ItemUpdateDto>();
            _mockItemService.Setup(s => s.Get(id)).Returns((Item)null!);

            var result = _controller.Update(id, updateDto);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
