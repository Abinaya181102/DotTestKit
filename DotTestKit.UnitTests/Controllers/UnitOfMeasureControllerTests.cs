using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMSAPI.Controllers;
using OMSAPI.Dtos.UnitOfMeasureDtos;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using Xunit;

namespace OMSAPI.Tests.Controllers
{
    public class UnitOfMeasureControllerTests
    {
        private readonly Mock<IUnitOfMeasure> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UnitOfMeasureController _controller;

        public UnitOfMeasureControllerTests()
        {
            _mockService = new Mock<IUnitOfMeasure>();
            _mockMapper = new Mock<IMapper>();
            _controller = new UnitOfMeasureController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetUnitOfMeasure_ReturnsOk_WhenFound()
        {
            var code = "KG";
            var uom = new UnitOfMeasure { Code = code };
            var dto = new UnitOfMeasureReadFullDto { Code = code };

            _mockService.Setup(s => s.Get(code)).Returns(uom);
            _mockMapper.Setup(m => m.Map<UnitOfMeasureReadFullDto>(uom)).Returns(dto);

            var result = _controller.GetUnitOfMeasure(code);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public void GetUnitOfMeasure_ReturnsNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.Get("XYZ")).Returns((UnitOfMeasure)null);

            var result = _controller.GetUnitOfMeasure("XYZ");

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetAll_ReturnsOkWithList()
        {
            var uoms = new List<UnitOfMeasure> { new UnitOfMeasure { Code = "KG" } };
            var dtos = new List<UnitOfMeasureReadDto> { new UnitOfMeasureReadDto { Code = "KG" } };

            _mockService.Setup(s => s.GetAll()).Returns(uoms);
            _mockMapper.Setup(m => m.Map<IEnumerable<UnitOfMeasureReadDto>>(uoms)).Returns(dtos);

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(dtos, okResult.Value);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRoute()
        {
            var createDto = new UnitOfMeasureCreateDto { Code = "KG" };
            var model = new UnitOfMeasure { Code = "KG" };
            var readDto = new UnitOfMeasureReadFullDto { Code = "KG" };

            _mockMapper.Setup(m => m.Map<UnitOfMeasure>(createDto)).Returns(model);
            _mockMapper.Setup(m => m.Map<UnitOfMeasureReadFullDto>(model)).Returns(readDto);

            var result = _controller.Create(createDto);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(readDto, createdResult.Value);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenFound()
        {
            var uom = new UnitOfMeasure { Code = "KG" };
            _mockService.Setup(s => s.Get("KG")).Returns(uom);

            var result = _controller.Delete("KG");

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.Get("ABC")).Returns((UnitOfMeasure)null);

            var result = _controller.Delete("ABC");

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenFound()
        {
            var updateDto = new UnitOfMeasureUpdateDto { Name = "Kilogram" };
            var uom = new UnitOfMeasure { Code = "KG", Name = "Old" };

            _mockService.Setup(s => s.Get("KG")).Returns(uom);

            var result = _controller.Update("KG", updateDto);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenNotFound()
        {
            _mockService.Setup(s => s.Get("XYZ")).Returns((UnitOfMeasure)null);

            var result = _controller.Update("XYZ", new UnitOfMeasureUpdateDto());

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
