/*using Xunit;
using Moq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using OMSAPI.Controllers;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using OMSAPI.Dtos.AddressDtos;
using AutoFixture;

namespace DotTestKit.UnitTests.Controllers
{
    public class AddressControllerTests
    {
        private readonly Mock<IAddress> _mockAddressService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddressController _controller;

        public AddressControllerTests()
        {
            _mockAddressService = new Mock<IAddress>();
            _mockMapper = new Mock<IMapper>();
            _controller = new AddressController(_mockAddressService.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetAddress_ReturnsNotFound_WhenAddressIsNull()
        {
            _mockAddressService.Setup(s => s.Get(It.IsAny<int>())).Returns((Address)null);

            var result = _controller.GetAddress(1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAddress_ReturnsOk_WhenAddressExists()
        {
            // Arrange
            var fixture = new Fixture();

            var address = fixture.Build<Address>()
                                 .With(a => a.Id, 1)
                                 .Create();

            var dto = fixture.Build<AddressReadFullDto>()
                             .With(d => d.Id, 1)
                             .Create();

            _mockAddressService.Setup(s => s.Get(1)).Returns(address);
            _mockMapper.Setup(m => m.Map<AddressReadFullDto>(address)).Returns(dto);

            // Act
            var result = _controller.GetAddress(1);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<AddressReadFullDto>()
                .Which.Id.Should().Be(1);
        }

        [Fact]
        public void GetAll_ReturnsAllAddresses()
        {
            var addresses = new List<Address> { new Address { Id = 1 }, new Address { Id = 2 } };
            var addressDtos = new List<AddressReadDto>
            {
                new AddressReadDto { Id = 1 },
                new AddressReadDto { Id = 2 }
            };

            _mockAddressService.Setup(s => s.GetAll()).Returns(addresses);
            _mockMapper.Setup(m => m.Map<IEnumerable<AddressReadDto>>(addresses)).Returns(addressDtos);

            var result = _controller.GetAll();

            result.Result.Should().BeOfType<OkObjectResult>()
               .Which.Value.Should().BeAssignableTo<IEnumerable<AddressReadDto>>();

        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenAddressExists()
        {
            // Arrange
            var address = new Address { Id = 1 };
            _mockAddressService.Setup(s => s.Get(1)).Returns(address);

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            _mockAddressService.Verify(s => s.Delete(address), Times.Once);
            _mockAddressService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            _mockAddressService.Setup(s => s.Get(1)).Returns((Address)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRouteResult_WhenValidInput()
        {
            // Arrange
            var createDto = new AddressCreateDto
            {
                Country = "India",
                PostCode = "600001",
                City = "Chennai",
                Street = "MG Road",
                BuildingNo = "12A",
                AppartmentNo = "2B",
                CustomerId = 101
            };

            var addressModel = new Address
            {
                Id = 1,
                Country = createDto.Country,
                PostCode = createDto.PostCode,
                City = createDto.City,
                Street = createDto.Street,
                BuildingNo = createDto.BuildingNo,
                AppartmentNo = createDto.AppartmentNo,
                CustomerId = createDto.CustomerId
            };

            var addressReadDto = new AddressReadFullDto
            {
                Id = 1,
                Country = "India",
                PostCode = "600001",
                City = "Chennai",
                Street = "MG Road",
                BuildingNo = "12A",
                AppartmentNo = "2B",
                CustomerId = 101
            };

            _mockMapper.Setup(m => m.Map<Address>(createDto)).Returns(addressModel);
            _mockMapper.Setup(m => m.Map<AddressReadFullDto>(addressModel)).Returns(addressReadDto);

            // Act
            var result = _controller.Create(createDto);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>()
               .Which.Value.Should().BeEquivalentTo(addressReadDto, options => options
               .ExcludingMissingMembers());


            _mockAddressService.Verify(s => s.Create(addressModel), Times.Once);
            _mockAddressService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenAddressExists()
        {
            // Arrange
            var updateDto = new AddressUpdateDto
            {
                Country = "India",
                PostCode = "600001",
                City = "Chennai",
                Street = "Anna Salai",
                BuildingNo = "10B",
                AppartmentNo = "3A",
                CustomerId = 202
            };

            var address = new Address
            {
                Id = 1,
                Country = "OldCountry",
                PostCode = "000000",
                City = "OldCity",
                Street = "OldStreet",
                BuildingNo = "OldNo",
                AppartmentNo = "OldFlat",
                CustomerId = 101
            };

            _mockAddressService.Setup(s => s.Get(1)).Returns(address);

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockMapper.Verify(m => m.Map(updateDto, address), Times.Once);
            _mockAddressService.Verify(s => s.Update(address), Times.Once);
            _mockAddressService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            _mockAddressService.Setup(s => s.Get(1)).Returns((Address)null);

            // Act
            var result = _controller.Update(1, new AddressUpdateDto
            {
                Country = "India",
                PostCode = "600001",
                City = "Chennai",
                Street = "MG Road",
                BuildingNo = "1",
                AppartmentNo = "1A",
                CustomerId = 101
            });

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetForCustomer_ReturnsAddressesForCustomer()
        {
            // Arrange
            int customerId = 101;
            var addresses = new List<Address>
    {
        new Address { Id = 1, CustomerId = customerId },
        new Address { Id = 2, CustomerId = customerId }
    };

            var addressDtos = new List<AddressReadDto>
    {
        new AddressReadDto { Id = 1 },
        new AddressReadDto { Id = 2 }
    };

            _mockAddressService.Setup(s => s.GetAllForCustomer(customerId)).Returns(addresses);
            _mockMapper.Setup(m => m.Map<IEnumerable<AddressReadDto>>(addresses)).Returns(addressDtos);

            // Act
            var result = _controller.GetForCustomer(customerId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new[]
                {
                     new AddressReadDto { Id = 1 },
                     new AddressReadDto { Id = 2 }
                });

        }


    }

}
*/



using Xunit;
using Moq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using OMSAPI.Controllers;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using OMSAPI.Dtos.AddressDtos;
using AutoFixture;

namespace DotTestKit.UnitTests.Controllers
{
    public class AddressControllerTests
    {
        private readonly Mock<IAddress> _mockAddressService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly AddressController _controller;
        private readonly Fixture _fixture;

        public AddressControllerTests()
        {
            _mockAddressService = new Mock<IAddress>();
            _mockMapper = new Mock<IMapper>();
            _controller = new AddressController(_mockAddressService.Object, _mockMapper.Object);
            _fixture = new Fixture();
        }

        [Fact]
        public void GetAddress_ShouldReturn_NotFound_WhenAddressIsNull()
        {
            // Arrange
            _mockAddressService.Setup(s => s.Get(It.IsAny<int>())).Returns((Address)null);

            // Act
            var result = _controller.GetAddress(1);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAddress_ShouldReturn_Ok_WhenAddressExists()
        {
            // Arrange
            var address = _fixture.Build<Address>().With(a => a.Id, 1).Create();
            var dto = _fixture.Build<AddressReadFullDto>().With(d => d.Id, 1).Create();

            _mockAddressService.Setup(s => s.Get(1)).Returns(address);
            _mockMapper.Setup(m => m.Map<AddressReadFullDto>(address)).Returns(dto);

            // Act
            var result = _controller.GetAddress(1);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeOfType<AddressReadFullDto>()
                  .Which.Id.Should().Be(1);
        }

        [Fact]
        public void GetAll_ShouldReturn_AllMappedAddresses()
        {
            // Arrange
            var addresses = new List<Address> { new() { Id = 1 }, new() { Id = 2 } };
            var dtos = new List<AddressReadDto> { new() { Id = 1 }, new() { Id = 2 } };

            _mockAddressService.Setup(s => s.GetAll()).Returns(addresses);
            _mockMapper.Setup(m => m.Map<IEnumerable<AddressReadDto>>(addresses)).Returns(dtos);

            // Act
            var result = _controller.GetAll();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                  .Which.Value.Should().BeEquivalentTo(dtos);
        }

        [Fact]
        public void Delete_ShouldReturn_NoContent_WhenAddressExists()
        {
            // Arrange
            var address = new Address { Id = 1 };
            _mockAddressService.Setup(s => s.Get(1)).Returns(address);

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockAddressService.Verify(s => s.Delete(address), Times.Once);
            _mockAddressService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ShouldReturn_NotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            _mockAddressService.Setup(s => s.Get(1)).Returns((Address)null);

            // Act
            var result = _controller.Delete(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Create_ShouldReturn_CreatedAtRoute_WhenInputIsValid()
        {
            // Arrange
            var createDto = _fixture.Build<AddressCreateDto>()
                .With(c => c.Country, "India")
                .Create();

            var addressModel = _fixture.Build<Address>()
                .With(a => a.Id, 1)
                .With(a => a.Country, createDto.Country)
                .Create();

            var readDto = _fixture.Build<AddressReadFullDto>()
                .With(a => a.Id, 1)
                .With(a => a.Country, createDto.Country)
                .Create();

            _mockMapper.Setup(m => m.Map<Address>(createDto)).Returns(addressModel);
            _mockMapper.Setup(m => m.Map<AddressReadFullDto>(addressModel)).Returns(readDto);

            // Act
            var result = _controller.Create(createDto);

            // Assert
            result.Should().BeOfType<CreatedAtRouteResult>()
                .Which.Value.Should().BeEquivalentTo(readDto, opts => opts.ExcludingMissingMembers());

            _mockAddressService.Verify(s => s.Create(addressModel), Times.Once);
            _mockAddressService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturn_NoContent_WhenAddressExists()
        {
            // Arrange
            var updateDto = _fixture.Create<AddressUpdateDto>();
            var existingAddress = _fixture.Build<Address>().With(a => a.Id, 1).Create();

            _mockAddressService.Setup(s => s.Get(1)).Returns(existingAddress);

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockMapper.Verify(m => m.Map(updateDto, existingAddress), Times.Once);
            _mockAddressService.Verify(s => s.Update(existingAddress), Times.Once);
            _mockAddressService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ShouldReturn_NotFound_WhenAddressDoesNotExist()
        {
            // Arrange
            _mockAddressService.Setup(s => s.Get(1)).Returns((Address)null);
            var updateDto = _fixture.Create<AddressUpdateDto>();

            // Act
            var result = _controller.Update(1, updateDto);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetForCustomer_ShouldReturn_AddressesMapped()
        {
            // Arrange
            int customerId = 101;

            var addresses = _fixture.Build<Address>()
                .With(a => a.CustomerId, customerId)
                .CreateMany(2)
                .ToList();

            var dtos = new List<AddressReadDto>
            {
                new() { Id = addresses[0].Id },
                new() { Id = addresses[1].Id }
            };

            _mockAddressService.Setup(s => s.GetAllForCustomer(customerId)).Returns(addresses);
            _mockMapper.Setup(m => m.Map<IEnumerable<AddressReadDto>>(addresses)).Returns(dtos);

            // Act
            var result = _controller.GetForCustomer(customerId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(dtos);
        }
    }
}
