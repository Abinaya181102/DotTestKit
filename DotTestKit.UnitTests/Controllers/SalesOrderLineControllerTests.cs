using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMSAPI.Controllers;
using OMSAPI.Dtos.SalesOrderLineDtos;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using System.Collections.Generic;
using Xunit;

namespace OMSAPI.Tests.Controllers
{
    public class SalesOrderLineControllerTests
    {
        private readonly Mock<ISalesOrderLine> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SalesOrderLineController _controller;

        public SalesOrderLineControllerTests()
        {
            _mockService = new Mock<ISalesOrderLine>();
            _mockMapper = new Mock<IMapper>();
            _controller = new SalesOrderLineController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetSalesOrderLine_ReturnsOk_WhenLineExists()
        {
            var id = 1;
            var line = new SalesOrderLine { Id = id };
            var dto = new SalesOrderLineReadFullDto { Id = id };
            _mockService.Setup(s => s.Get(id)).Returns(line);
            _mockMapper.Setup(m => m.Map<SalesOrderLineReadFullDto>(line)).Returns(dto);

            var result = _controller.GetSalesOrderLine(id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public void GetSalesOrderLine_ReturnsNotFound_WhenLineNotExists()
        {
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderLine)null);

            var result = _controller.GetSalesOrderLine(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetAll_ReturnsAllLines()
        {
            var lines = new List<SalesOrderLine> { new SalesOrderLine { Id = 1 } };
            var dtos = new List<SalesOrderLineReadDto> { new SalesOrderLineReadDto { Id = 1 } };
            _mockService.Setup(s => s.GetAll()).Returns(lines);
            _mockMapper.Setup(m => m.Map<IEnumerable<SalesOrderLineReadDto>>(lines)).Returns(dtos);

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dtos, okResult.Value);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRoute()
        {
            var createDto = new SalesOrderLineCreateDto();
            var model = new SalesOrderLine { Id = 1 };
            var readDto = new SalesOrderLineReadFullDto { Id = 1 };

            _mockMapper.Setup(m => m.Map<SalesOrderLine>(createDto)).Returns(model);
            _mockMapper.Setup(m => m.Map<SalesOrderLineReadFullDto>(model)).Returns(readDto);

            var result = _controller.Create(createDto);

            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("SalesOrderLine", createdAtRouteResult.RouteName);
            Assert.Equal(readDto, createdAtRouteResult.Value);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenExists()
        {
            var line = new SalesOrderLine { Id = 1 };
            _mockService.Setup(s => s.Get(1)).Returns(line);

            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.Get(1)).Returns((SalesOrderLine)null);

            var result = _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenExists()
        {
            var dto = new SalesOrderLineUpdateDto();
            var model = new SalesOrderLine { Id = 1 };
            _mockService.Setup(s => s.Get(1)).Returns(model);

            var result = _controller.Update(1, dto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenNotExists()
        {
            _mockService.Setup(s => s.Get(1)).Returns((SalesOrderLine)null);

            var result = _controller.Update(1, new SalesOrderLineUpdateDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetAllForSalesOrderHeader_ReturnsLines()
        {
            var lines = new List<SalesOrderLine> { new SalesOrderLine { Id = 1 } };
            var dtos = new List<SalesOrderLineReadDto> { new SalesOrderLineReadDto { Id = 1 } };
            _mockService.Setup(s => s.GetAllForSalesOrder(It.IsAny<int>())).Returns(lines);
            _mockMapper.Setup(m => m.Map<IEnumerable<SalesOrderLineReadDto>>(lines)).Returns(dtos);

            var result = _controller.GetAllForSalesOrderHeader(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dtos, okResult.Value);
        }
    }
}
