
//using System.Collections.Generic;
//using AutoFixture;
//using AutoMapper;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using OMSAPI.Controllers;
//using OMSAPI.Dtos.AddressDtos;
//using OMSAPI.Interfaces;
//using OMSAPI.Models;
//using OMSAPI.Profiles;
//using Xunit;

//namespace OMSAPI.UnitTests.Controllers
//{
//    public class AddressControllerTests
//    {
//        private readonly Mock<IAddress> _mockAddressService;
//        private readonly IMapper _mapper;
//        private readonly Fixture _fixture;
//        private readonly AddressController _controller;

//        public AddressControllerTests()
//        {
//            _mockAddressService = new Mock<IAddress>();
//            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AddressProfile()));
//            _mapper = config.CreateMapper();
//            _fixture = new Fixture();
//            _controller = new AddressController(_mockAddressService.Object, _mapper);
//        }

//        [Fact]
//        public void GetAddress_ReturnsOk_WhenAddressExists()
//        {
//            var address = _fixture.Create<Address>();
//            _mockAddressService.Setup(s => s.Get(address.Id)).Returns(address);

//            var result = _controller.GetAddress(address.Id);

//            result.Result.Should().BeOfType<OkObjectResult>();
//        }

//        [Fact]
//        public void GetAddress_ReturnsNotFound_WhenAddressDoesNotExist()
//        {
//            _mockAddressService.Setup(s => s.Get(It.IsAny<int>())).Returns((Address)null);

//            var result = _controller.GetAddress(1);

//            result.Result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void Create_ReturnsCreatedAtRoute_WhenValidDataPassed()
//        {
//            var createDto = _fixture.Create<AddressCreateDto>();
//            var result = _controller.Create(createDto);
//            result.Should().BeOfType<CreatedAtRouteResult>();
//        }

//        [Fact]
//        public void Delete_ReturnsNoContent_WhenDeleted()
//        {
//            var address = _fixture.Create<Address>();
//            _mockAddressService.Setup(s => s.Get(address.Id)).Returns(address);

//            var result = _controller.Delete(address.Id);

//            result.Should().BeOfType<NoContentResult>();
//        }

//        [Fact]
//        public void Update_ReturnsNoContent_WhenUpdated()
//        {
//            var address = _fixture.Create<Address>();
//            var updateDto = _mapper.Map<AddressUpdateDto>(address);
//            _mockAddressService.Setup(s => s.Get(address.Id)).Returns(address);

//            var result = _controller.Update(address.Id, updateDto);

//            result.Should().BeOfType<NoContentResult>();
//        }
//    }
//}

using System.Collections.Generic;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMSAPI.Controllers;
using OMSAPI.Dtos.AddressDtos;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using OMSAPI.Profiles;
using Xunit;

namespace OMSAPI.UnitTests.Controllers
{
    public class AddressControllerTests
    {
        private readonly Mock<IAddress> _mockAddressService;
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;
        private readonly AddressController _controller;

        public AddressControllerTests()
        {
            _mockAddressService = new Mock<IAddress>();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AddressProfile()));
            _mapper = config.CreateMapper();
            _fixture = new Fixture();
            _controller = new AddressController(_mockAddressService.Object, _mapper);
        }

        [Fact]
        public void GetAddress_ReturnsOk_WhenAddressExists()
        {
            var address = _fixture.Create<Address>();
            _mockAddressService.Setup(s => s.Get(address.Id)).Returns(address);

            var result = _controller.GetAddress(address.Id);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetAddress_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            _mockAddressService.Setup(s => s.Get(It.IsAny<int>())).Returns((Address)null);

            var result = _controller.GetAddress(1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAll_ReturnsOk_WithListOfAddresses()
        {
            var addresses = _fixture.CreateMany<Address>(3);
            _mockAddressService.Setup(s => s.GetAll()).Returns(addresses);

            var result = _controller.GetAll();

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GetForCustomer_ReturnsOk_WithCustomerAddresses()
        {
            var addresses = _fixture.CreateMany<Address>(2);
            _mockAddressService.Setup(s => s.GetAllForCustomer(It.IsAny<int>())).Returns(addresses);

            var result = _controller.GetForCustomer(1);

            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void Create_ReturnsCreatedAtRoute_WhenValidDataPassed()
        {
            var createDto = _fixture.Create<AddressCreateDto>();

            var result = _controller.Create(createDto);

            result.Should().BeOfType<CreatedAtRouteResult>();
            _mockAddressService.Verify(x => x.Create(It.IsAny<Address>()), Times.Once);
            _mockAddressService.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenAddressExists()
        {
            var address = _fixture.Create<Address>();
            _mockAddressService.Setup(s => s.Get(address.Id)).Returns(address);

            var result = _controller.Delete(address.Id);

            result.Should().BeOfType<NoContentResult>();
            _mockAddressService.Verify(x => x.Delete(address), Times.Once);
            _mockAddressService.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            _mockAddressService.Setup(s => s.Get(It.IsAny<int>())).Returns((Address)null);

            var result = _controller.Delete(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenAddressExists()
        {
            var address = _fixture.Create<Address>();
            var updateDto = _mapper.Map<AddressUpdateDto>(address);

            _mockAddressService.Setup(s => s.Get(address.Id)).Returns(address);

            var result = _controller.Update(address.Id, updateDto);

            result.Should().BeOfType<NoContentResult>();
            _mockAddressService.Verify(x => x.Update(address), Times.Once);
            _mockAddressService.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            var updateDto = _fixture.Create<AddressUpdateDto>();
            _mockAddressService.Setup(s => s.Get(It.IsAny<int>())).Returns((Address)null);

            var result = _controller.Update(1, updateDto);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
