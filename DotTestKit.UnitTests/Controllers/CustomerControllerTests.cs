


////using System.Collections.Generic;
////using AutoFixture;
////using AutoMapper;
////using FluentAssertions;
////using Microsoft.AspNetCore.Mvc;
////using Moq;
////using OMSAPI.Controllers;
////using OMSAPI.Dtos.CustomerDtos;
////using OMSAPI.Interfaces;
////using OMSAPI.Models;
////using OMSAPI.Profiles;
////using Xunit;

////namespace OMSAPI.UnitTests.Controllers
////{
////    public class CustomerControllerTests
////    {
////        private readonly Mock<ICustomer> _mockCustomerService;
////        private readonly IMapper _mapper;
////        private readonly Fixture _fixture;
////        private readonly CustomerController _controller;

////        public CustomerControllerTests()
////        {
////            _mockCustomerService = new Mock<ICustomer>();
////            var config = new MapperConfiguration(cfg => cfg.AddProfile(new CustomerProfile()));
////            _mapper = config.CreateMapper();
////            _controller = new CustomerController(_mockCustomerService.Object, _mapper);
////            _fixture = new Fixture();
////            _fixture.Behaviors
////                .OfType<ThrowingRecursionBehavior>()
////                .ToList()
////                .ForEach(b => _fixture.Behaviors.Remove(b));
////            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

////        }

////        [Fact]
////        public void GetCustomer_ReturnsOk_WhenCustomerExists()
////        {
////            var customer = _fixture.Create<Customer>();
////            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

////            var result = _controller.GetCustomer(customer.Id);

////            result.Result.Should().BeOfType<OkObjectResult>();
////        }

////        [Fact]
////        public void GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
////        {
////            _mockCustomerService.Setup(s => s.Get(It.IsAny<int>())).Returns((Customer)null);

////            var result = _controller.GetCustomer(1);

////            result.Result.Should().BeOfType<NotFoundResult>();
////        }

////        [Fact]
////        public void Create_ReturnsCreatedAtRoute_WhenValidDataPassed()
////        {
////            var createDto = _fixture.Create<CustomerCreateDto>();
////            var result = _controller.Create(createDto);
////            result.Should().BeOfType<CreatedAtRouteResult>();
////        }

////        [Fact]
////        public void Delete_ReturnsNoContent_WhenDeleted()
////        {
////            var customer = _fixture.Create<Customer>();
////            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

////            var result = _controller.Delete(customer.Id);

////            result.Should().BeOfType<NoContentResult>();
////        }

////        [Fact]
////        public void Update_ReturnsNoContent_WhenUpdated()
////        {
////            var customer = _fixture.Create<Customer>();
////            var updateDto = _mapper.Map<CustomerUpdateDto>(customer);
////            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

////            var result = _controller.Update(customer.Id, updateDto);

////            result.Should().BeOfType<NoContentResult>();
////        }
////    }
////}


//using AutoFixture;
//using AutoMapper;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using OMSAPI.Controllers;
//using OMSAPI.Dtos.CustomerDtos;
//using OMSAPI.Interfaces;
//using OMSAPI.Models;
//using OMSAPI.Profiles;
//namespace OMSAPI.UnitTests.Controllers
//{
//    public class CustomerControllerTests
//    {
//        private readonly Mock<ICustomer> _mockCustomerService;
//        private readonly IMapper _mapper;
//        private readonly Fixture _fixture;
//        private readonly CustomerController _controller;

//        public CustomerControllerTests()
//        {
//            _mockCustomerService = new Mock<ICustomer>();

//            var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerProfile>());
//            _mapper = config.CreateMapper();

//            _controller = new CustomerController(_mockCustomerService.Object, _mapper);

//            _fixture = new Fixture();
//            _fixture.Behaviors
//                .OfType<ThrowingRecursionBehavior>()
//                .ToList()
//                .ForEach(b => _fixture.Behaviors.Remove(b));
//            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//        }

//        [Fact]
//        public void GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
//        {
//            _mockCustomerService.Setup(s => s.Get(It.IsAny<int>())).Returns((Customer)null);

//            var result = _controller.GetCustomer(1);

//            result.Result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void Create_ReturnsCreatedAtRoute_WhenValidDataPassed()
//        {
//            // Arrange
//            var createDto = _fixture.Create<CustomerCreateDto>();
//            var customerModel = _mapper.Map<Customer>(createDto);
//            var customerReadFullDto = _mapper.Map<CustomerReadFullDto>(customerModel);

//            _mockCustomerService.Setup(s => s.Create(It.IsAny<Customer>()));
//            _mockCustomerService.Setup(s => s.SaveChanges());

//            // Act
//            var result = _controller.Create(createDto);

//            // Assert
//            result.Should().BeOfType<CreatedAtRouteResult>();
//            _mockCustomerService.Verify(s => s.Create(It.IsAny<Customer>()), Times.Once);
//            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public void Delete_ReturnsNoContent_WhenDeleted()
//        {
//            var customer = _fixture.Create<Customer>();
//            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

//            var result = _controller.Delete(customer.Id);

//            result.Should().BeOfType<NoContentResult>();
//            _mockCustomerService.Verify(s => s.Delete(customer), Times.Once);
//            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public void Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
//        {
//            _mockCustomerService.Setup(s => s.Get(It.IsAny<int>())).Returns((Customer)null);

//            var result = _controller.Delete(999);

//            result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void Update_ReturnsNoContent_WhenUpdated()
//        {
//            var customer = _fixture.Create<Customer>();
//            var updateDto = _mapper.Map<CustomerUpdateDto>(customer);

//            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

//            var result = _controller.Update(customer.Id, updateDto);

//            result.Should().BeOfType<NoContentResult>();
//            _mockCustomerService.Verify(s => s.Update(customer), Times.Once);
//            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public void Update_ReturnsNotFound_WhenCustomerDoesNotExist()
//        {
//            _mockCustomerService.Setup(s => s.Get(It.IsAny<int>())).Returns((Customer)null);

//            var updateDto = _fixture.Create<CustomerUpdateDto>();

//            var result = _controller.Update(999, updateDto);

//            result.Should().BeOfType<NotFoundResult>();
//        }
//    }
//}


//using System.Collections.Generic;
//using System.Linq;
//using AutoFixture;
//using AutoMapper;
//using FluentAssertions;
//using Microsoft.AspNetCore.Mvc;
//using Moq;
//using OMSAPI.Controllers;
//using OMSAPI.Dtos.CustomerDtos;
//using OMSAPI.Interfaces;
//using OMSAPI.Models;
//using OMSAPI.Profiles;
//using Xunit;

//namespace OMSAPI.UnitTests.Controllers
//{
//    public class CustomerControllerTests
//    {
//        private readonly Mock<ICustomer> _mockCustomerService;
//        private readonly IMapper _mapper;
//        private readonly Fixture _fixture;
//        private readonly CustomerController _controller;

//        public CustomerControllerTests()
//        {
//            _mockCustomerService = new Mock<ICustomer>();

//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile<CustomerProfile>();
//            });
//            _mapper = config.CreateMapper();

//            _controller = new CustomerController(_mockCustomerService.Object, _mapper);

//            _fixture = new Fixture();
//            _fixture.Behaviors
//                .OfType<ThrowingRecursionBehavior>()
//                .ToList()
//                .ForEach(b => _fixture.Behaviors.Remove(b));
//            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
//        }

//        [Fact]
//        public void GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
//        {
//            _mockCustomerService.Setup(s => s.Get(It.IsAny<int>())).Returns((Customer)null);

//            var result = _controller.GetCustomer(1);

//            result.Result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void GetAll_ReturnsOk_WithCustomerList()
//        {
//            var customers = _fixture.CreateMany<Customer>(3).ToList();
//            _mockCustomerService.Setup(s => s.GetAll()).Returns(customers);

//            var result = _controller.GetAll();

//            result.Result.Should().BeOfType<OkObjectResult>();
//            var okResult = result.Result as OkObjectResult;
//            okResult!.Value.Should().BeAssignableTo<IEnumerable<CustomerReadDto>>();
//        }

//        [Fact]
//        public void Create_ReturnsCreatedAtRoute_WhenValidDataPassed()
//        {
//            var createDto = _fixture.Create<CustomerCreateDto>();
//            var customerModel = _mapper.Map<Customer>(createDto);

//            _mockCustomerService.Setup(s => s.Create(It.IsAny<Customer>()));
//            _mockCustomerService.Setup(s => s.SaveChanges());

//            var result = _controller.Create(createDto);

//            result.Should().BeOfType<CreatedAtRouteResult>();
//            var createdAt = result as CreatedAtRouteResult;
//            createdAt!.Value.Should().BeOfType<CustomerReadFullDto>();
//            _mockCustomerService.Verify(s => s.Create(It.IsAny<Customer>()), Times.Once);
//            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public void Delete_ReturnsNoContent_WhenCustomerExists()
//        {
//            var customer = _fixture.Create<Customer>();
//            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

//            var result = _controller.Delete(customer.Id);

//            result.Should().BeOfType<NoContentResult>();
//            _mockCustomerService.Verify(s => s.Delete(customer), Times.Once);
//            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public void Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
//        {
//            _mockCustomerService.Setup(s => s.Get(It.IsAny<int>())).Returns((Customer)null);

//            var result = _controller.Delete(999);

//            result.Should().BeOfType<NotFoundResult>();
//        }

//        [Fact]
//        public void Update_ReturnsNoContent_WhenCustomerExists()
//        {
//            var customer = _fixture.Create<Customer>();
//            var updateDto = _mapper.Map<CustomerUpdateDto>(customer);

//            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

//            var result = _controller.Update(customer.Id, updateDto);

//            result.Should().BeOfType<NoContentResult>();
//            _mockCustomerService.Verify(s => s.Update(customer), Times.Once);
//            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
//        }

//        [Fact]
//        public void Update_ReturnsNotFound_WhenCustomerDoesNotExist()
//        {
//            _mockCustomerService.Setup(s => s.Get(It.IsAny<int>())).Returns((Customer)null);
//            var updateDto = _fixture.Create<CustomerUpdateDto>();

//            var result = _controller.Update(999, updateDto);

//            result.Should().BeOfType<NotFoundResult>();
//        }
//    }
//}



using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OMSAPI.Controllers;
using OMSAPI.Dtos.CustomerDtos;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using OMSAPI.Profiles;
using Xunit;

namespace OMSAPI.UnitTests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomer> _mockCustomerService;
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockCustomerService = new Mock<ICustomer>();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerProfile>());
            _mapper = config.CreateMapper();

            _controller = new CustomerController(_mockCustomerService.Object, _mapper);

            _fixture = new Fixture();
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public void GetCustomer_ReturnsOk_WhenCustomerExists()
        {
            var customer = _fixture.Create<Customer>();
            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

            var result = _controller.GetCustomer(customer.Id);

            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeOfType<CustomerReadFullDto>();
        }

        [Fact]
        public void GetCustomer_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            var randomId = _fixture.Create<int>();
            _mockCustomerService.Setup(s => s.Get(randomId)).Returns((Customer)null);

            var result = _controller.GetCustomer(randomId);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void GetAll_ReturnsOk_WithCustomerList()
        {
            var customers = _fixture.CreateMany<Customer>(3).ToList();
            _mockCustomerService.Setup(s => s.GetAll()).Returns(customers);

            var result = _controller.GetAll();

            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult!.Value.Should().BeAssignableTo<IEnumerable<CustomerReadDto>>();
        }

        [Fact]
        public void GetAll_ReturnsOk_WhenListIsEmpty()
        {
            _mockCustomerService.Setup(s => s.GetAll()).Returns(new List<Customer>());

            var result = _controller.GetAll();

            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;

            okResult!.Value.Should().BeAssignableTo<IEnumerable<CustomerReadDto>>();

            var customerDtos = okResult.Value as IEnumerable<CustomerReadDto>;
            customerDtos.Should().BeEmpty();
        }

        [Fact]
        public void Create_ReturnsCreatedAtRoute_WhenValidDataPassed()
        {
            var createDto = _fixture.Create<CustomerCreateDto>();
            var mappedCustomer = _mapper.Map<Customer>(createDto);

            _mockCustomerService.Setup(s => s.Create(It.IsAny<Customer>()));
            _mockCustomerService.Setup(s => s.SaveChanges());

            var result = _controller.Create(createDto);

            result.Should().BeOfType<CreatedAtRouteResult>();
            var createdAt = result as CreatedAtRouteResult;
            createdAt!.Value.Should().BeOfType<CustomerReadFullDto>();
            _mockCustomerService.Verify(s => s.Create(It.Is<Customer>(c => c != null)), Times.Once);
            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenCustomerExists()
        {
            var customer = _fixture.Create<Customer>();
            _mockCustomerService.Setup(s => s.Get(customer.Id)).Returns(customer);

            var result = _controller.Delete(customer.Id);

            result.Should().BeOfType<NoContentResult>();
            _mockCustomerService.Verify(s => s.Delete(customer), Times.Once);
            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            var randomId = _fixture.Create<int>();
            _mockCustomerService.Setup(s => s.Get(randomId)).Returns((Customer)null);

            var result = _controller.Delete(randomId);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenCustomerExists()
        {
            var existingCustomer = _fixture.Create<Customer>();
            var updateDto = _mapper.Map<CustomerUpdateDto>(existingCustomer);

            _mockCustomerService.Setup(s => s.Get(existingCustomer.Id)).Returns(existingCustomer);

            var result = _controller.Update(existingCustomer.Id, updateDto);

            result.Should().BeOfType<NoContentResult>();
            _mockCustomerService.Verify(s => s.Update(existingCustomer), Times.Once);
            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            var updateDto = _fixture.Create<CustomerUpdateDto>();
            var randomId = _fixture.Create<int>();

            _mockCustomerService.Setup(s => s.Get(randomId)).Returns((Customer)null);

            var result = _controller.Update(randomId, updateDto);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
