using System.Collections.Generic;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMSAPI.Controllers;
using OMSAPI.Dtos.SalesOrderLineDtos;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using Xunit;

namespace OMSAPI.UnitTests.Controllers
{
    public class SalesOrderLineControllerTests
    {
        private readonly Mock<ISalesOrderLine> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly SalesOrderLineController _controller;
        private readonly IFixture _fixture;

        public SalesOrderLineControllerTests()
        {
            _mockService = new Mock<ISalesOrderLine>();
            _mockMapper = new Mock<IMapper>();
            _controller = new SalesOrderLineController(_mockService.Object, _mockMapper.Object);
            _fixture = new Fixture();
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        }

        [Fact]
        public void GetSalesOrderLine_ReturnsOk_WhenSalesOrderLineExists()
        {
            var entity = _fixture.Create<SalesOrderLine>();
            var dto = _fixture.Create<SalesOrderLineReadFullDto>();

            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns(entity);
            _mockMapper.Setup(m => m.Map<SalesOrderLineReadFullDto>(entity)).Returns(dto);

            var result = _controller.GetSalesOrderLine(1);

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public void GetSalesOrderLine_ReturnsNotFound_WhenEntityNull()
        {
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderLine)null);

            var result = _controller.GetSalesOrderLine(1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAll_ReturnsListOfDtos()
        {
            var entities = _fixture.CreateMany<SalesOrderLine>(3);
            var dtos = _fixture.CreateMany<SalesOrderLineReadDto>(3);

            _mockService.Setup(s => s.GetAll()).Returns(entities);
            _mockMapper.Setup(m => m.Map<IEnumerable<SalesOrderLineReadDto>>(It.IsAny<IEnumerable<SalesOrderLine>>())).Returns(dtos);

            var result = _controller.GetAll();

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(dtos);
        }

        [Fact]
        public void GetAllForSalesOrderHeader_ReturnsListOfDtos()
        {
            var entities = _fixture.CreateMany<SalesOrderLine>(2);
            var dtos = _fixture.CreateMany<SalesOrderLineReadDto>(2);

            _mockService.Setup(s => s.GetAllForSalesOrder(It.IsAny<int>())).Returns(entities);
            _mockMapper.Setup(m => m.Map<IEnumerable<SalesOrderLineReadDto>>(It.IsAny<IEnumerable<SalesOrderLine>>())).Returns(dtos);

            var result = _controller.GetAllForSalesOrderHeader(1);

            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(dtos);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRoute_WhenValid()
        {
            var createDto = _fixture.Create<SalesOrderLineCreateDto>();
            var model = _fixture.Build<SalesOrderLine>()
                                .With(x => x.Id, 1)
                                .Create();
            var readDto = _fixture.Build<SalesOrderLineReadFullDto>()
                                  .With(x => x.Id, 1)
                                  .Create();

            _mockMapper.Setup(m => m.Map<SalesOrderLine>(createDto)).Returns(model);
            _mockService.Setup(s => s.Create(model));
            _mockService.Setup(s => s.SaveChanges()).Returns(true);
            _mockMapper.Setup(m => m.Map<SalesOrderLineReadFullDto>(model)).Returns(readDto);

            var result = _controller.Create(createDto);

            result.Should().BeOfType<CreatedAtRouteResult>()
                  .Which.Value.Should().BeEquivalentTo(readDto);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenEntityExists()
        {
            var entity = _fixture.Create<SalesOrderLine>();
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns(entity);

            var result = _controller.Delete(entity.Id);

            _mockService.Verify(s => s.Delete(entity), Times.Once);
            _mockService.Verify(s => s.SaveChanges(), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenEntityDoesNotExist()
        {
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderLine)null);

            var result = _controller.Delete(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenValid()
        {
            var updateDto = _fixture.Create<SalesOrderLineUpdateDto>();
            var entity = _fixture.Create<SalesOrderLine>();

            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns(entity);

            var result = _controller.Update(entity.Id, updateDto);

            _mockMapper.Verify(m => m.Map(updateDto, entity), Times.Once);
            _mockService.Verify(s => s.Update(entity), Times.Once);
            _mockService.Verify(s => s.SaveChanges(), Times.Once);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenEntityDoesNotExist()
        {
            var updateDto = _fixture.Create<SalesOrderLineUpdateDto>();
            _mockService.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderLine)null);

            var result = _controller.Update(1, updateDto);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}