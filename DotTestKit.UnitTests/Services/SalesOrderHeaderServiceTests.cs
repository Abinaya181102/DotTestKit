//using FluentAssertions;
//using OMSAPI.DataContext;
//using OMSAPI.Models;
//using OMSAPI.Services;
//using OMSAPI.UnitTests.TestHelpers;
//using System.Linq;
//using Xunit;

//namespace OMSAPI.UnitTests.Services
//{
//    public class SalesOrderHeaderServiceTests
//    {
//        private readonly SalesOrderHeaderService _service;
//        private readonly OMSDbContext _context;

//        public SalesOrderHeaderServiceTests()
//        {
//            _context = TestDbContextFactory.CreateInMemoryDbContext();
//            _service = new SalesOrderHeaderService(_context);
//        }

//        [Fact]
//        public void Create_ShouldAddHeader()
//        {
//            var header = new SalesOrderHeader();
//            _service.Create(header);
//            _service.SaveChanges();

//            _context.SalesOrderHeaders.Should().Contain(header);
//        }

//        [Fact]
//        public void Get_ShouldReturnHeader()
//        {
//            var header = new SalesOrderHeader();
//            _context.SalesOrderHeaders.Add(header);
//            _context.SaveChanges();

//            var result = _service.Get(header.Id);

//            result.Should().BeEquivalentTo(header, options => options.ExcludingMissingMembers());
//        }

//        [Fact]
//        public void GetAll_ShouldReturnHeaders()
//        {
//            _context.SalesOrderHeaders.Add(new SalesOrderHeader());
//            _context.SalesOrderHeaders.Add(new SalesOrderHeader());
//            _context.SaveChanges();

//            var result = _service.GetAll();

//            result.Count().Should().BeGreaterThan(1);
//        }

//        [Fact]
//        public void Update_ShouldSetEntityState()
//        {
//            var header = new SalesOrderHeader { OrderDate = System.DateTime.Now };
//            _context.SalesOrderHeaders.Add(header);
//            _context.SaveChanges();

//            header.Profit = 100.0M;
//            _service.Update(header);
//            _service.SaveChanges();

//            _context.SalesOrderHeaders.First().Profit.Should().Be(100.0M);
//        }

//        [Fact]
//        public void Delete_ShouldRemoveHeader()
//        {
//            var header = new SalesOrderHeader();
//            _context.SalesOrderHeaders.Add(header);
//            _context.SaveChanges();

//            _service.Delete(header);
//            _service.SaveChanges();

//            _context.SalesOrderHeaders.Should().NotContain(header);
//        }
//    }
//}


using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using OMSAPI.UnitTests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace OMSAPI.UnitTests.Services
{
    public class SalesOrderHeaderServiceTests
    {
        private readonly SalesOrderHeaderService _service;
        private readonly OMSDbContext _context;

        public SalesOrderHeaderServiceTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _service = new SalesOrderHeaderService(_context);
        }

        [Fact]
        public void Create_ShouldAddHeader()
        {
            var header = new SalesOrderHeader();
            _service.Create(header);
            _service.SaveChanges();

            _context.SalesOrderHeaders.Should().Contain(header);
        }

        [Fact]
        public void Create_ShouldThrow_WhenNull()
        {
            Action act = () => _service.Create(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        //[Fact]
        //public void Get_ShouldReturnHeader_WithIncludes()
        //{
        //    var customer = new Customer();
        //    var address = new Address();
        //    var item = new Item();
        //    var line = new SalesOrderLine { Item = item };
        //    var header = new SalesOrderHeader
        //    {
        //        Customer = customer,
        //        Address = address,
        //        Lines = new List<SalesOrderLine> { line }
        //    };

        //    _context.Customers.Add(customer);
        //    _context.Addresses.Add(address);
        //    _context.Items.Add(item);
        //    _context.SalesOrderHeaders.Add(header);
        //    _context.SaveChanges();

        //    var result = _service.Get(header.Id);

        //    result.Should().NotBeNull();
        //    result.Customer.Should().NotBeNull();
        //    result.Address.Should().NotBeNull();
        //    result.Lines.Should().ContainSingle();
        //    result.Lines.First().Item.Should().NotBeNull();
        //}

        //[Fact]
        //public void GetAll_ShouldReturnAllWithIncludes()
        //{
        //    var customer = new Customer();
        //    var address = new Address();
        //    var header = new SalesOrderHeader
        //    {
        //        Customer = customer,
        //        Address = address
        //    };

        //    _context.Customers.Add(customer);
        //    _context.Addresses.Add(address);
        //    _context.SalesOrderHeaders.Add(header);
        //    _context.SaveChanges();

        //    var result = _service.GetAll().ToList();

        //    result.Should().HaveCount(1);
        //    result[0].Customer.Should().NotBeNull();
        //    result[0].Address.Should().NotBeNull();
        //}

        [Fact]
        public void Update_ShouldSetModifiedState()
        {
            var header = new SalesOrderHeader { Profit = 10 };
            _context.SalesOrderHeaders.Add(header);
            _context.SaveChanges();

            header.Profit = 123;
            _service.Update(header);
            _context.Entry(header).State.Should().Be(EntityState.Modified);
        }

        [Fact]
        public void Delete_ShouldRemoveHeader()
        {
            var header = new SalesOrderHeader();
            _context.SalesOrderHeaders.Add(header);
            _context.SaveChanges();

            _service.Delete(header);
            _service.SaveChanges();

            _context.SalesOrderHeaders.Should().NotContain(header);
        }

        [Fact]
        public void Delete_ShouldThrow_WhenNull()
        {
            Action act = () => _service.Delete(null!);

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void SaveChanges_ShouldReturnTrue_WhenChangesMade()
        {
            var header = new SalesOrderHeader();
            _context.SalesOrderHeaders.Add(header);

            var result = _service.SaveChanges();

            result.Should().BeTrue();
        }

        //[Fact]
        //public void UpdateProfit_ShouldReturnTrue_WhenCallSucceeds()
        //{
        //    // InMemory does not support stored procedures, so mock success
        //    var result = _service.UpdateProfit(1);

        //    result.Should().BeTrue(); // InMemory will skip actual execution
        //}

        [Fact]
        public void UpdateProfit_ShouldReturnFalse_WhenCallFails()
        {
            var faultyContext = TestDbContextFactory.CreateInMemoryDbContext(); // fresh context
            var service = new SalesOrderHeaderService(faultyContext);

            // Simulate exception by disposing DB first (forces access exception)
            faultyContext.Dispose();

            var result = service.UpdateProfit(1);

            result.Should().BeFalse();
        }
    }
}