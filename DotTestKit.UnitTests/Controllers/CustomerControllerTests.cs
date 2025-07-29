using Xunit;
using Moq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using OMSAPI.Controllers;
using OMSAPI.Interfaces;
using OMSAPI.Models;
using OMSAPI.Dtos.CustomerDtos;

namespace DotTestKit.UnitTests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomer> _mockCustomerService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockCustomerService = new Mock<ICustomer>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CustomerController(_mockCustomerService.Object, _mockMapper.Object);
        }

        [Fact]
        public void GetCustomer_ReturnsNotFound_WhenCustomerIsNull()
        {
            _mockCustomerService.Setup(s => s.Get(It.IsAny<int>())).Returns((Customer)null);

            var result = _controller.GetCustomer(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCustomer_ReturnsOk_WhenCustomerExists()
        {
            var customer = new Customer { Id = 1 };
            var dto = new CustomerReadFullDto { Id = 1 };

            _mockCustomerService.Setup(s => s.Get(1)).Returns(customer);
            _mockMapper.Setup(m => m.Map<CustomerReadFullDto>(customer)).Returns(dto);

            var result = _controller.GetCustomer(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<CustomerReadFullDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public void GetAll_ReturnsAllCustomers()
        {
            var customers = new List<Customer> { new Customer { Id = 1 }, new Customer { Id = 2 } };
            var customerDtos = new List<CustomerReadDto>
            {
                new CustomerReadDto { Id = 1 },
                new CustomerReadDto { Id = 2 }
            };

            _mockCustomerService.Setup(s => s.GetAll()).Returns(customers);
            _mockMapper.Setup(m => m.Map<IEnumerable<CustomerReadDto>>(customers)).Returns(customerDtos);

            var result = _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<CustomerReadDto>>(okResult.Value);
        }

        [Fact]
        public void Create_ReturnsCreatedAtRoute_WhenValidInput()
        {
            var createDto = new CustomerCreateDto { Name = "John Doe" };
            var customerModel = new Customer { Id = 1, Name = "John Doe" };
            var customerReadDto = new CustomerReadFullDto { Id = 1, Name = "John Doe" };

            _mockMapper.Setup(m => m.Map<Customer>(createDto)).Returns(customerModel);
            _mockMapper.Setup(m => m.Map<CustomerReadFullDto>(customerModel)).Returns(customerReadDto);

            var result = _controller.Create(createDto);

            var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<CustomerReadFullDto>(createdResult.Value);
            Assert.Equal(customerReadDto.Id, returnValue.Id);

            _mockCustomerService.Verify(s => s.Create(customerModel), Times.Once);
            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ReturnsNoContent_WhenCustomerExists()
        {
            var updateDto = new CustomerUpdateDto { Name = "Updated Name" };
            var customer = new Customer { Id = 1, Name = "Old Name" };

            _mockCustomerService.Setup(s => s.Get(1)).Returns(customer);

            var result = _controller.Update(1, updateDto);

            Assert.IsType<NoContentResult>(result);
            _mockMapper.Verify(m => m.Map(updateDto, customer), Times.Once);
            _mockCustomerService.Verify(s => s.Update(customer), Times.Once);
            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Update_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            _mockCustomerService.Setup(s => s.Get(1)).Returns((Customer)null);

            var result = _controller.Update(1, new CustomerUpdateDto { Name = "Name" });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_ReturnsNoContent_WhenCustomerExists()
        {
            var customer = new Customer { Id = 1 };
            _mockCustomerService.Setup(s => s.Get(1)).Returns(customer);

            var result = _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            _mockCustomerService.Verify(s => s.Delete(customer), Times.Once);
            _mockCustomerService.Verify(s => s.SaveChanges(), Times.Once);
        }

        [Fact]
        public void Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
        {
            _mockCustomerService.Setup(s => s.Get(1)).Returns((Customer)null);

            var result = _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
