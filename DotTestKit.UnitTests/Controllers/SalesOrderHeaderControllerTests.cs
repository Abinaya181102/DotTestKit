//using AutoFixture;
//using AutoMapper;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using OMSAPI.Controllers;
//using OMSAPI.Dtos.SalesOrderHeaderDtos;
//using OMSAPI.Interfaces;
//using OMSAPI.Models;
//using System.Collections.Generic;
//using Xunit;

//namespace OMSAPI.UnitTests.Controllers
//{
//    public class SalesOrderHeaderControllerTests
//    {
//        private readonly Mock<ISalesOrderHeader> _serviceMock;
//        private readonly Mock<IMapper> _mapperMock;
//        private readonly SalesOrderHeaderController _controller;
//        private readonly Fixture _fixture;

//        public SalesOrderHeaderControllerTests()
//        {
//            _serviceMock = new Mock<ISalesOrderHeader>();
//            _mapperMock = new Mock<IMapper>();
//            _controller = new SalesOrderHeaderController(_serviceMock.Object, _mapperMock.Object);
//            _fixture = new Fixture();
//            _fixture.Behaviors
//                .OfType<ThrowingRecursionBehavior>()
//                .ToList()
//                .ForEach(b => _fixture.Behaviors.Remove(b));
//            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//        }

//        [Fact]
//        public void GetSalesOrderHeader_ShouldReturnOk_WhenItemExists()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            var dto = _fixture.Create<SalesOrderHeaderReadFullDto>();

//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);
//            _mapperMock.Setup(m => m.Map<SalesOrderHeaderReadFullDto>(model)).Returns(dto);

//            var result = _controller.GetSalesOrderHeader(model.Id);
//            result.Result.Should().BeOfType<OkObjectResult>()
//                .Which.Value.Should().BeEquivalentTo(dto);
//        }

//        [Fact]
//        public void GetSalesOrderHeader_ShouldReturnNotFound_WhenItemMissing()
//        {
//            _serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(value: null);
//            var result = _controller.GetSalesOrderHeader(1);
//            result.Result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void GetAll_ShouldReturnMappedList()
//        {
//            var models = _fixture.CreateMany<SalesOrderHeader>(3);
//            var dtos = _fixture.CreateMany<SalesOrderHeaderReadDto>(3);

//            _serviceMock.Setup(s => s.GetAll()).Returns(models);
//            _mapperMock.Setup(m => m.Map<IEnumerable<SalesOrderHeaderReadDto>>(models)).Returns(dtos);

//            var result = _controller.GetAll();
//            result.Result.Should().BeOfType<OkObjectResult>()
//                .Which.Value.Should().BeEquivalentTo(dtos);
//        }

//        [Fact]
//        public void Create_ShouldReturnCreatedAtRoute()
//        {
//            var createDto = _fixture.Create<SalesOrderHeaderCreateDto>();
//            var model = _fixture.Build<SalesOrderHeader>().Without(x => x.Customer).Without(x => x.Address).Create();
//            var fullDto = _fixture.Create<SalesOrderHeaderReadFullDto>();

//            _mapperMock.Setup(m => m.Map<SalesOrderHeader>(createDto)).Returns(model);
//            _mapperMock.Setup(m => m.Map<SalesOrderHeaderReadFullDto>(model)).Returns(fullDto);

//            var result = _controller.Create(createDto);
//            result.Should().BeOfType<CreatedAtRouteResult>()
//                .Which.Value.Should().BeEquivalentTo(fullDto);
//        }

//        [Fact]
//        public void Delete_ShouldReturnNoContent_WhenItemExists()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

//            var result = _controller.Delete(model.Id);
//            result.Should().BeOfType<NoContentResult>();
//        }

//        [Fact]
//        public void Delete_ShouldReturnNotFound_WhenItemMissing()
//        {
//            _serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderHeader)null);
//            var result = _controller.Delete(1);
//            result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void Update_ShouldReturnNoContent_WhenSuccessful()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            var updateDto = _fixture.Create<SalesOrderHeaderUpdateDto>();

//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);
//            _mapperMock.Setup(m => m.Map(updateDto, model));

//            var result = _controller.Update(model.Id, updateDto);
//            result.Should().BeOfType<NoContentResult>();
//        }

//        [Fact]
//        public void UpdateProfit_ShouldReturnUpdatedHeader()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            _serviceMock.Setup(s => s.UpdateProfit(model.Id)).Returns(true);
//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

//            var result = _controller.UpdateProfit(model.Id);
//            result.Value.Should().BeEquivalentTo(model);
//        }

//        [Fact]
//        public void UpdateProfit_ShouldSetProfitToZero_OnFailure()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            _serviceMock.Setup(s => s.UpdateProfit(model.Id)).Returns(false);
//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

//            var result = _controller.UpdateProfit(model.Id);
//            result.Value.Profit.Should().Be(0.0m);
//        }
//    }
//}




//using AutoFixture;
//using AutoMapper;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using OMSAPI.Controllers;
//using OMSAPI.Dtos.SalesOrderHeaderDtos;
//using OMSAPI.Interfaces;
//using OMSAPI.Models;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace OMSAPI.UnitTests.Controllers
//{
//    public class SalesOrderHeaderControllerTests
//    {
//        private readonly Mock<ISalesOrderHeader> _serviceMock;
//        private readonly Mock<IMapper> _mapperMock;
//        private readonly SalesOrderHeaderController _controller;
//        private readonly Fixture _fixture;

//        public SalesOrderHeaderControllerTests()
//        {
//            _serviceMock = new Mock<ISalesOrderHeader>();
//            _mapperMock = new Mock<IMapper>();
//            _controller = new SalesOrderHeaderController(_serviceMock.Object, _mapperMock.Object);
//            _fixture = new Fixture();
//            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
//                .ForEach(b => _fixture.Behaviors.Remove(b));
//            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//        }

//        [Fact]
//        public void GetSalesOrderHeader_ShouldReturnOk_WhenItemExists()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            var dto = _fixture.Create<SalesOrderHeaderReadFullDto>();
//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);
//            _mapperMock.Setup(m => m.Map<SalesOrderHeaderReadFullDto>(model)).Returns(dto);

//            var result = _controller.GetSalesOrderHeader(model.Id);
//            result.Result.Should().BeOfType<OkObjectResult>()
//                .Which.Value.Should().BeEquivalentTo(dto);
//        }

//        [Fact]
//        public void GetSalesOrderHeader_ShouldReturnNotFound_WhenItemMissing()
//        {
//            _serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderHeader)null);

//            var result = _controller.GetSalesOrderHeader(1);
//            result.Result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void GetAll_ShouldReturnMappedList_WhenItemsExist()
//        {
//            var models = _fixture.CreateMany<SalesOrderHeader>(3).ToList();
//            var dtos = _fixture.CreateMany<SalesOrderHeaderReadDto>(3).ToList();
//            _serviceMock.Setup(s => s.GetAll()).Returns(models);
//            _mapperMock.Setup(m => m.Map<IEnumerable<SalesOrderHeaderReadDto>>(models)).Returns(dtos);

//            var result = _controller.GetAll();
//            result.Result.Should().BeOfType<OkObjectResult>()
//                .Which.Value.Should().BeEquivalentTo(dtos);
//        }

//        [Fact]
//        public void GetAll_ShouldReturnEmptyList_WhenNoItemsExist()
//        {
//            var models = new List<SalesOrderHeader>();
//            var dtos = new List<SalesOrderHeaderReadDto>();
//            _serviceMock.Setup(s => s.GetAll()).Returns(models);
//            _mapperMock.Setup(m => m.Map<IEnumerable<SalesOrderHeaderReadDto>>(models)).Returns(dtos);

//            var result = _controller.GetAll();
//            result.Result.Should().BeOfType<OkObjectResult>()
//                .Which.Value.Should().BeEquivalentTo(dtos);
//        }

//        [Fact]
//        public void Create_ShouldReturnCreatedAtRoute()
//        {
//            var createDto = _fixture.Create<SalesOrderHeaderCreateDto>();
//            var model = _fixture.Build<SalesOrderHeader>().Without(x => x.Customer).Without(x => x.Address).Create();
//            var fullDto = _fixture.Create<SalesOrderHeaderReadFullDto>();

//            _mapperMock.Setup(m => m.Map<SalesOrderHeader>(createDto)).Returns(model);
//            _mapperMock.Setup(m => m.Map<SalesOrderHeaderReadFullDto>(model)).Returns(fullDto);

//            var result = _controller.Create(createDto);

//            var createdAt = result.Should().BeOfType<CreatedAtRouteResult>().Subject;
//            createdAt.Value.Should().BeEquivalentTo(fullDto);
//            createdAt.RouteName.Should().Be("GetSalesOrderHeader");
//            createdAt.RouteValues["id"].Should().Be(fullDto.Id);
//        }

//        [Fact]
//        public void Delete_ShouldReturnNoContent_WhenItemExists()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

//            var result = _controller.Delete(model.Id);
//            result.Should().BeOfType<NoContentResult>();
//        }

//        [Fact]
//        public void Delete_ShouldReturnNotFound_WhenItemMissing()
//        {
//            _serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderHeader)null);

//            var result = _controller.Delete(123);
//            result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void Update_ShouldReturnNoContent_WhenSuccessful()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            var updateDto = _fixture.Create<SalesOrderHeaderUpdateDto>();
//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

//            var result = _controller.Update(model.Id, updateDto);

//            result.Should().BeOfType<NoContentResult>();
//            _mapperMock.Verify(m => m.Map(updateDto, model), Times.Once);
//            _serviceMock.Verify(s => s.Update(model), Times.Once);
//            _serviceMock.Verify(s => s.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public void Update_ShouldReturnNotFound_WhenItemMissing()
//        {
//            _serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderHeader)null);
//            var updateDto = _fixture.Create<SalesOrderHeaderUpdateDto>();

//            var result = _controller.Update(99, updateDto);

//            result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void UpdateProfit_ShouldReturnUpdatedHeader_WhenSuccessful()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            _serviceMock.Setup(s => s.UpdateProfit(model.Id)).Returns(true);
//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

//            var result = _controller.UpdateProfit(model.Id);
//            result.Value.Should().BeEquivalentTo(model);
//        }

//        [Fact]
//        public void UpdateProfit_ShouldSetProfitToZero_WhenFailed()
//        {
//            var model = _fixture.Create<SalesOrderHeader>();
//            _serviceMock.Setup(s => s.UpdateProfit(model.Id)).Returns(false);
//            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

//            var result = _controller.UpdateProfit(model.Id);
//            result.Value.Profit.Should().Be(0.0m);
//        }
//    }
//}



using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMSAPI.Controllers;
using OMSAPI.Dtos.SalesOrderHeaderDtos;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OMSAPI.UnitTests.Controllers
{
    public class SalesOrderHeaderControllerTests
    {
        private readonly Mock<ISalesOrderHeader> _serviceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly SalesOrderHeaderController _controller;
        private readonly Fixture _fixture;

        public SalesOrderHeaderControllerTests()
        {
            _serviceMock = new Mock<ISalesOrderHeader>();
            _mapperMock = new Mock<IMapper>();
            _controller = new SalesOrderHeaderController(_serviceMock.Object, _mapperMock.Object);
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void GetSalesOrderHeader_ShouldReturnOk_WhenItemExists()
        {
            var model = _fixture.Create<SalesOrderHeader>();
            var dto = _fixture.Create<SalesOrderHeaderReadFullDto>();
            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);
            _mapperMock.Setup(m => m.Map<SalesOrderHeaderReadFullDto>(model)).Returns(dto);

            var result = _controller.GetSalesOrderHeader(model.Id);
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public void GetSalesOrderHeader_ShouldReturnNotFound_WhenItemMissing()
        {
            _serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderHeader)null);

            var result = _controller.GetSalesOrderHeader(1);
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAll_ShouldReturnMappedList_WhenItemsExist()
        {
            var models = _fixture.CreateMany<SalesOrderHeader>(3).ToList();
            var dtos = _fixture.CreateMany<SalesOrderHeaderReadDto>(3).ToList();
            _serviceMock.Setup(s => s.GetAll()).Returns(models);
            _mapperMock.Setup(m => m.Map<IEnumerable<SalesOrderHeaderReadDto>>(models)).Returns(dtos);

            var result = _controller.GetAll();
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(dtos);
        }

        [Fact]
        public void GetAll_ShouldReturnEmptyList_WhenNoItemsExist()
        {
            var models = new List<SalesOrderHeader>();
            var dtos = new List<SalesOrderHeaderReadDto>();
            _serviceMock.Setup(s => s.GetAll()).Returns(models);
            _mapperMock.Setup(m => m.Map<IEnumerable<SalesOrderHeaderReadDto>>(models)).Returns(dtos);

            var result = _controller.GetAll();
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(dtos);
        }

        [Fact]
        public void Create_ShouldReturnCreatedAtRoute()
        {
            var createDto = _fixture.Create<SalesOrderHeaderCreateDto>();
            var model = _fixture.Build<SalesOrderHeader>().Without(x => x.Customer).Without(x => x.Address).Create();
            var fullDto = _fixture.Create<SalesOrderHeaderReadFullDto>();

            _mapperMock.Setup(m => m.Map<SalesOrderHeader>(createDto)).Returns(model);
            _mapperMock.Setup(m => m.Map<SalesOrderHeaderReadFullDto>(model)).Returns(fullDto);

            var result = _controller.Create(createDto);

            var createdAt = result.Should().BeOfType<CreatedAtRouteResult>().Subject;
            createdAt.Value.Should().BeEquivalentTo(fullDto);
            createdAt.RouteName.Should().Be("GetSalesOrderHeader");
            createdAt.RouteValues["id"].Should().Be(fullDto.Id);
        }

        [Fact]
        public void Delete_ShouldReturnNoContent_WhenItemExists()
        {
            var model = _fixture.Create<SalesOrderHeader>();
            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

            var result = _controller.Delete(model.Id);
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public void Delete_ShouldReturnNotFound_WhenItemMissing()
        {
            _serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderHeader)null);

            var result = _controller.Delete(123);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Update_ShouldReturnNoContent_WhenSuccessful()
        {
            var model = _fixture.Create<SalesOrderHeader>();
            var updateDto = _fixture.Create<SalesOrderHeaderUpdateDto>();
            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

            var result = _controller.Update(model.Id, updateDto);

            result.Should().BeOfType<NoContentResult>();
            _mapperMock.Verify(m => m.Map(updateDto, model), Times.Once);
            _serviceMock.Verify(s => s.Update(model), Times.Once);
            _serviceMock.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturnNotFound_WhenItemMissing()
        {
            _serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns((SalesOrderHeader)null);
            var updateDto = _fixture.Create<SalesOrderHeaderUpdateDto>();

            var result = _controller.Update(99, updateDto);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void UpdateProfit_ShouldReturnUpdatedHeader_WhenSuccessful()
        {
            var model = _fixture.Create<SalesOrderHeader>();
            _serviceMock.Setup(s => s.UpdateProfit(model.Id)).Returns(true);
            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

            var result = _controller.UpdateProfit(model.Id);
            result.Value.Should().BeEquivalentTo(model);
        }

        [Fact]
        public void UpdateProfit_ShouldSetProfitToZero_WhenFailed()
        {
            var model = _fixture.Create<SalesOrderHeader>();
            _serviceMock.Setup(s => s.UpdateProfit(model.Id)).Returns(false);
            _serviceMock.Setup(s => s.Get(model.Id)).Returns(model);

            var result = _controller.UpdateProfit(model.Id);
            result.Value.Profit.Should().Be(0.0m);
        }
    }
}
