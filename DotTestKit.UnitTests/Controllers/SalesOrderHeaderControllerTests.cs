using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using OMSAPI.Controllers;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using OMSAPI.Dtos.SalesOrderHeaderDtos;

namespace DotTestKit.UnitTests.Controllers
{
    public class SalesOrderHeaderControllerTests
    {
        private readonly Mock<ISalesOrderHeader> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SalesOrderHeaderController _controller;

        public SalesOrderHeaderControllerTests()
        {
            _mockService = new Mock<ISalesOrderHeader>();
            _mockMapper = new Mock<IMapper>();
            _controller = new SalesOrderHeaderController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetSalesOrderHeader_ReturnsNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.Get(1)).Returns((SalesOrderHeader)null);

            var result = _controller.GetSalesOrderHeader(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetSalesOrderHeader_ReturnsOk_WhenExists()
        {
            var model = new SalesOrderHeader { Id = 1 };
            var dto = new SalesOrderHeaderReadFullDto { Id = 1 };

            _mockService.Setup(s => s.Get(1)).Returns(model);
            _mockMapper.Setup(m => m.Map<SalesOrderHeaderReadFullDto>(model)).Returns(dto);

            var result = _controller.GetSalesOrderHeader(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto.Id, ((SalesOrderHeaderReadFullDto)okResult.Value).Id);
        }

        [Fact]
        public void GetAll_ReturnsAll()
        {
            var list = new List<SalesOrderHeader> { new SalesOrderHeader { Id = 1 } };
            var dtoList = new List<SalesOrderHeaderReadDto> { new SalesOrderHeaderReadDto { Id = 1 } };

            _mockService.Setup(s => s.GetAll()).Returns(list);
            _mockMapper.Setup(m => m.Map<IEnumerable<SalesOrderHeaderReadDto>>(list)).Returns(dtoList);

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.IsAssignableFrom<IEnumerable<SalesOrderHeaderReadDto>>(okResult.Value);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRoute()
        {
            var createDto = new SalesOrderHeaderCreateDto();
            var model = new SalesOrderHeader { Id = 1 };
            var fullDto = new SalesOrderHeaderReadFullDto { Id = 1 };

            _mockMapper.Setup(m => m.Map<SalesOrderHeader>(createDto)).Returns(model);
            _mockMapper.Setup(m => m.Map<SalesOrderHeaderReadFullDto>(model)).Returns(fullDto);

            var result = _controller.Create(createDto);

            var createdAtRoute = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("GetSalesOrderHeader", createdAtRoute.RouteName);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.Get(1)).Returns((SalesOrderHeader)null);

            var result = _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenDeleted()
        {
            var model = new SalesOrderHeader { Id = 1 };

            _mockService.Setup(s => s.Get(1)).Returns(model);

            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.Get(1)).Returns((SalesOrderHeader)null);

            var result = _controller.Update(1, new SalesOrderHeaderUpdateDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenUpdated()
        {
            var model = new SalesOrderHeader { Id = 1 };

            _mockService.Setup(s => s.Get(1)).Returns(model);

            var result = _controller.Update(1, new SalesOrderHeaderUpdateDto());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateProfit_ReturnsOrder_WithProfit()
        {
            var model = new SalesOrderHeader { Id = 1, Profit = 500 };

            _mockService.Setup(s => s.UpdateProfit(1)).Returns(true);
            _mockService.Setup(s => s.Get(1)).Returns(model);

            var result = _controller.UpdateProfit(1);

            var returnValue = Assert.IsType<ActionResult<SalesOrderHeader>>(result);
            Assert.Equal(500, returnValue.Value.Profit);
        }

        [Fact]
        public void UpdateProfit_ReturnsOrder_WithZeroProfit_WhenUpdateFails()
        {
            var model = new SalesOrderHeader { Id = 1 };

            _mockService.Setup(s => s.UpdateProfit(1)).Returns(false);
            _mockService.Setup(s => s.Get(1)).Returns(model);

            var result = _controller.UpdateProfit(1);

            Assert.Equal(0.0M, result.Value.Profit);
        }
    }
}
