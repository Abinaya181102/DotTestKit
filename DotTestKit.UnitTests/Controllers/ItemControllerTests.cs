using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using OMSAPI.Controllers;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using OMSAPI.Dtos.ItemDtos;

namespace DotTestKit.UnitTests.Controllers
{
    public class ItemControllerTests
    {
        private readonly Mock<IItem> _mockItemService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ItemController _controller;

        public ItemControllerTests()
        {
            _mockItemService = new Mock<IItem>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ItemController(_mockItemService.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetItem_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockItemService.Setup(s => s.Get(It.IsAny<int>())).Returns((Item)null);

            var result = _controller.GetItem(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetItem_ReturnsOk_WhenItemExists()
        {
            var item = new Item { Id = 1 };
            var dto = new ItemReadFullDto { Id = 1 };

            _mockItemService.Setup(s => s.Get(1)).Returns(item);
            _mockMapper.Setup(m => m.Map<ItemReadFullDto>(item)).Returns(dto);

            var result = _controller.GetItem(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ItemReadFullDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void GetAll_ReturnsAllItems()
        {
            var items = new List<Item> { new Item { Id = 1 }, new Item { Id = 2 } };
            var dtos = new List<ItemReadDto>
            {
                new ItemReadDto { Id = 1 },
                new ItemReadDto { Id = 2 }
            };

            _mockItemService.Setup(s => s.GetAll()).Returns(items);
            _mockMapper.Setup(m => m.Map<IEnumerable<ItemReadDto>>(items)).Returns(dtos);

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<ItemReadDto>>(okResult.Value);
            Assert.Collection(returnValue,
                i => Assert.Equal(1, i.Id),
                i => Assert.Equal(2, i.Id));
        }

        [Fact]
        public void Create_ReturnsCreatedAtRoute_WhenValidInput()
        {
            var createDto = new ItemCreateDto { Name = "New Item" };
            var itemModel = new Item { Id = 1, Name = "New Item" };
            var readDto = new ItemReadFullDto { Id = 1, Name = "New Item" };

            _mockMapper.Setup(m => m.Map<Item>(createDto)).Returns(itemModel);
            _mockMapper.Setup(m => m.Map<ItemReadFullDto>(itemModel)).Returns(readDto);

            var result = _controller.Create(createDto);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<ItemReadFullDto>(createdResult.Value);
            Assert.Equal(readDto.Id, returnValue.Id);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockItemService.Setup(s => s.Get(1)).Returns((Item)null);

            var result = _controller.Update(1, new ItemUpdateDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenItemExists()
        {
            var item = new Item { Id = 1 };
            var updateDto = new ItemUpdateDto { Name = "Updated Item" };

            _mockItemService.Setup(s => s.Get(1)).Returns(item);

            var result = _controller.Update(1, updateDto);

            Assert.IsType<NoContentResult>(result);
            _mockMapper.Verify(m => m.Map(updateDto, item), Times.Once);
            _mockItemService.Verify(s => s.Update(item), Times.Once);
            _mockItemService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenItemDoesNotExist()
        {
            _mockItemService.Setup(s => s.Get(1)).Returns((Item)null);

            var result = _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenItemExists()
        {
            var item = new Item { Id = 1 };

            _mockItemService.Setup(s => s.Get(1)).Returns(item);

            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            _mockItemService.Verify(s => s.Delete(item), Times.Once);
            _mockItemService.Verify(s => s.SaveChanges(), Times.Once);
        }
    }
}
