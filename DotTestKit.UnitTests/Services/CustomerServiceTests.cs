//using AutoFixture;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using OMSAPI.DataContext;
//using OMSAPI.Models;
//using OMSAPI.Services;
//using OMSAPI.UnitTests.TestHelpers;
//using System;
//using System.Linq;
//using Xunit;

//namespace OMSAPI.UnitTests.Services
//{
//    public class CustomerServiceTests
//    {
//        private readonly CustomerService _service;
//        private readonly Fixture _fixture;
//        private readonly OMSDbContext _context;

//        public CustomerServiceTests()
//        {
//            _context = TestDbContextFactory.CreateInMemoryDbContext();
//            _service = new CustomerService(_context);
//            _fixture = new Fixture();
//        }

//        [Fact]
//        public void Create_AddsCustomerToContext()
//        {
//            var customer = _fixture.Create<Customer>();
//            _service.Create(customer);
//            _service.SaveChanges();

//            _context.Customers.Should().Contain(customer);
//        }

//        [Fact]
//        public void GetAll_ReturnsAllCustomers()
//        {
//            var customer = _fixture.Create<Customer>();
//            _context.Customers.Add(customer);
//            _context.SaveChanges();

//            var result = _service.GetAll();
//            result.Should().Contain(customer);
//        }

//        [Fact]
//        public void Get_ReturnsCorrectCustomer()
//        {
//            var customer = _fixture.Create<Customer>();
//            _context.Customers.Add(customer);
//            _context.SaveChanges();

//            var result = _service.Get(customer.Id);
//            result.Should().BeEquivalentTo(customer);
//        }

//        [Fact]
//        public void Delete_RemovesCustomer()
//        {
//            var customer = _fixture.Create<Customer>();
//            _context.Customers.Add(customer);
//            _context.SaveChanges();

//            _service.Delete(customer);
//            _service.SaveChanges();

//            _context.Customers.Should().NotContain(customer);
//        }
//    }
//}


using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using OMSAPI.UnitTests.TestHelpers;
using System;
using System.Linq;
using Xunit;

namespace OMSAPI.UnitTests.Services
{
    public class CustomerServiceTests
    {
        private readonly CustomerService _service;
        private readonly Fixture _fixture;
        private readonly OMSDbContext _context;

        public CustomerServiceTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _service = new CustomerService(_context);
            _fixture = new Fixture();
        }

        [Fact]
        public void Create_AddsCustomerToContext()
        {
            var customer = _fixture.Create<Customer>();

            _service.Create(customer);
            _service.SaveChanges();

            _context.Customers.Should().ContainEquivalentOf(customer);
        }

        [Fact]
        public void Create_ThrowsArgumentNullException_WhenCustomerIsNull()
        {
            Action act = () => _service.Create(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetAll_ReturnsAllCustomers()
        {
            var customers = _fixture.CreateMany<Customer>(3).ToList();
            _context.Customers.AddRange(customers);
            _context.SaveChanges();

            var result = _service.GetAll();

            result.Should().BeEquivalentTo(customers);
        }

        [Fact]
        public void Get_ReturnsCorrectCustomer_WhenExists()
        {
            var customer = _fixture.Create<Customer>();
            _context.Customers.Add(customer);
            _context.SaveChanges();

            var result = _service.Get(customer.Id);

            result.Should().BeEquivalentTo(customer);
        }

        [Fact]
        public void Get_ReturnsNull_WhenNotExists()
        {
            var result = _service.Get(-1);

            result.Should().BeNull();
        }

        [Fact]
        public void Delete_RemovesCustomer_WhenExists()
        {
            var customer = _fixture.Create<Customer>();
            _context.Customers.Add(customer);
            _context.SaveChanges();

            _service.Delete(customer);
            _service.SaveChanges();

            _context.Customers.Should().NotContain(customer);
        }

        [Fact]
        public void Delete_ThrowsArgumentNullException_WhenCustomerIsNull()
        {
            Action act = () => _service.Delete(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_ChangesEntityStateToModified()
        {
            var customer = _fixture.Create<Customer>();
            _context.Customers.Add(customer);
            _context.SaveChanges();

            _service.Update(customer);
            _context.Entry(customer).State.Should().Be(EntityState.Modified);
        }

        [Fact]
        public void SaveChanges_ReturnsTrue_WhenChangesSaved()
        {
            var customer = _fixture.Create<Customer>();
            _context.Customers.Add(customer);

            var result = _service.SaveChanges();

            result.Should().BeTrue();
        }
    }
}
