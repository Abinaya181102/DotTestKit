using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMSAPI.Controllers;
using OMSAPI.Dtos.UnitOfMeasureDtos;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using System.Collections.Generic;
using Xunit;

namespace OMSAPI.UnitTests.Controllers
{
    public class UnitOfMeasureControllerTests
    {
        private readonly Mock<IUnitOfMeasure> _serviceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly UnitOfMeasureController _controller;
        private readonly Fixture _fixture;

        public UnitOfMeasureControllerTests()
        {
            _fixture = new Fixture();
            _serviceMock = new Mock<IUnitOfMeasure>();
            _mapperMock = new Mock<IMapper>();
            _controller = new UnitOfMeasureController(_serviceMock.Object, _mapperMock.Object);
        }

        [Fact]
        public void GetUnitOfMeasure_ShouldReturnOk_WhenExists()
        {
            var entity = _fixture.Create<UnitOfMeasure>();
            var dto = _fixture.Create<UnitOfMeasureReadFullDto>();

            _serviceMock.Setup(s => s.Get(entity.Code)).Returns(entity);
            _mapperMock.Setup(m => m.Map<UnitOfMeasureReadFullDto>(entity)).Returns(dto);

            var result = _controller.GetUnitOfMeasure(entity.Code);
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public void GetUnitOfMeasure_ShouldReturnNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.Get(It.IsAny<string>())).Returns((UnitOfMeasure)null);
            var result = _controller.GetUnitOfMeasure("NON_EXISTENT_CODE");
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAll_ShouldReturnAllMappedDtos()
        {
            var models = _fixture.CreateMany<UnitOfMeasure>(3);
            var dtos = _fixture.CreateMany<UnitOfMeasureReadDto>(3);

            _serviceMock.Setup(s => s.GetAll()).Returns(models);
            _mapperMock.Setup(m => m.Map<IEnumerable<UnitOfMeasureReadDto>>(models)).Returns(dtos);

            var result = _controller.GetAll();
            result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(dtos);
        }

        [Fact]
        public void Create_ShouldReturnCreatedAtRoute()
        {
            var createDto = _fixture.Create<UnitOfMeasureCreateDto>();
            var model = _fixture.Create<UnitOfMeasure>();
            var fullDto = _fixture.Create<UnitOfMeasureReadFullDto>();

            _mapperMock.Setup(m => m.Map<UnitOfMeasure>(createDto)).Returns(model);
            _mapperMock.Setup(m => m.Map<UnitOfMeasureReadFullDto>(model)).Returns(fullDto);

            var result = _controller.Create(createDto);
            result.Should().BeOfType<CreatedAtRouteResult>().Which.Value.Should().BeEquivalentTo(fullDto);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent_WhenFound()
        {
            var model = _fixture.Create<UnitOfMeasure>();
            _serviceMock.Setup(s => s.Get(model.Code)).Returns(model);

            var result = _controller.Delete(model.Code);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.Get(It.IsAny<string>())).Returns((UnitOfMeasure)null);
            var result = _controller.Delete("NON_EXISTENT_CODE");
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Update_ShouldReturnNoContent_WhenSuccessful()
        {
            var model = _fixture.Create<UnitOfMeasure>();
            var updateDto = _fixture.Create<UnitOfMeasureUpdateDto>();

            _serviceMock.Setup(s => s.Get(model.Code)).Returns(model);

            var result = _controller.Update(model.Code, updateDto);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Update_ShouldReturnNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.Get(It.IsAny<string>())).Returns((UnitOfMeasure)null);
            var updateDto = _fixture.Create<UnitOfMeasureUpdateDto>();

            var result = _controller.Update("NON_EXISTENT_CODE", updateDto);
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}