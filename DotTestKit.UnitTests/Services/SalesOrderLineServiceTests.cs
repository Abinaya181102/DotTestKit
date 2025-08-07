//using System;
//using System.Linq;
//using FluentAssertions;
//using Microsoft.EntityFrameworkCore;
//using OMSAPI.Models;
//using OMSAPI.Services;
//using OMSAPI.UnitTests.TestHelpers;
//using Xunit;

//namespace OMSAPI.UnitTests.Services
//{
//    public class SalesOrderLineServiceTests
//    {
//        [Fact]
//        public void Create_AddsToContext()
//        {
//            using var context = TestDbContextFactory.CreateInMemoryDbContext();
//            var service = new SalesOrderLineService(context);
//            var entity = new SalesOrderLine { Quantity = 5, Amount = 100, ItemId = 1, SalesOrderHeaderId = 1 };

//            service.Create(entity);
//            var result = service.SaveChanges();

//            result.Should().BeTrue();
//            context.SalesOrderLines.Count().Should().Be(1);
//        }

//        [Fact]
//        public void Get_ReturnsEntityById()
//        {
//            using var context = TestDbContextFactory.CreateInMemoryDbContext();

//            var item = new Item { Id = 2, Name = "TestItem" };
//            context.Items.Add(item);

//            var entity = new SalesOrderLine { Quantity = 3, Amount = 50, ItemId = item.Id, SalesOrderHeaderId = 1 };
//            context.SalesOrderLines.Add(entity);
//            context.SaveChanges();

//            var service = new SalesOrderLineService(context);
//            var result = service.Get(entity.Id);

//            result.Should().NotBeNull();
//            result.Id.Should().Be(entity.Id);
//            result.Item.Should().NotBeNull();
//        }

//        [Fact]
//        public void Create_ThrowsException_WhenEntityIsNull()
//        {
//            using var context = TestDbContextFactory.CreateInMemoryDbContext();
//            var service = new SalesOrderLineService(context);

//            Action act = () => service.Create(null);
//            act.Should().Throw<ArgumentNullException>();
//        }

//        [Fact]
//        public void Delete_ThrowsException_WhenEntityIsNull()
//        {
//            using var context = TestDbContextFactory.CreateInMemoryDbContext();
//            var service = new SalesOrderLineService(context);

//            Action act = () => service.Delete(null);
//            act.Should().Throw<ArgumentNullException>();
//        }

//        [Fact]
//        public void Get_ReturnsNull_WhenIdDoesNotExist()
//        {
//            using var context = TestDbContextFactory.CreateInMemoryDbContext();
//            var service = new SalesOrderLineService(context);

//            var result = service.Get(999);
//            result.Should().BeNull();
//        }

//        [Fact]
//        public void GetAll_ReturnsEmpty_WhenNoData()
//        {
//            using var context = TestDbContextFactory.CreateInMemoryDbContext();
//            var service = new SalesOrderLineService(context);

//            var result = service.GetAll();
//            result.Should().BeEmpty();
//        }

//        [Fact]
//        public void GetAllForSalesOrder_ReturnsEmpty_WhenNoMatch()
//        {
//            using var context = TestDbContextFactory.CreateInMemoryDbContext();
//            var service = new SalesOrderLineService(context);

//            var result = service.GetAllForSalesOrder(999);
//            result.Should().BeEmpty();
//        }
//    }
//}


using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using OMSAPI.UnitTests.TestHelpers;
using Xunit;

namespace OMSAPI.UnitTests.Services
{
    public class SalesOrderLineServiceTests
    {
        private readonly OMSDbContext _context;
        private readonly SalesOrderLineService _service;

        public SalesOrderLineServiceTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _service = new SalesOrderLineService(_context);
        }

        [Fact]
        public void Create_ShouldAddLine()
        {
            var line = new SalesOrderLine { Quantity = 1, Amount = 10, ItemId = 1, SalesOrderHeaderId = 1 };
            _service.Create(line);
            var result = _service.SaveChanges();

            result.Should().BeTrue();
            _context.SalesOrderLines.Should().Contain(line);
        }

        [Fact]
        public void Create_ShouldThrow_WhenNull()
        {
            Action act = () => _service.Create(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Delete_ShouldRemoveLine()
        {
            var line = new SalesOrderLine { Quantity = 1, Amount = 10 };
            _context.SalesOrderLines.Add(line);
            _context.SaveChanges();

            _service.Delete(line);
            _service.SaveChanges();

            _context.SalesOrderLines.Should().NotContain(line);
        }

        [Fact]
        public void Delete_ShouldThrow_WhenNull()
        {
            Action act = () => _service.Delete(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Get_ShouldReturnLineWithItem()
        {
            var item = new Item { Name = "Item1" };
            _context.Items.Add(item);

            var line = new SalesOrderLine { Quantity = 2, Amount = 20, Item = item };
            _context.SalesOrderLines.Add(line);
            _context.SaveChanges();

            var result = _service.Get(line.Id);

            result.Should().NotBeNull();
            result!.Item.Should().NotBeNull();
            result.Id.Should().Be(line.Id);
        }

        [Fact]
        public void Get_ShouldReturnNull_WhenNotFound()
        {
            var result = _service.Get(999);
            result.Should().BeNull();
        }

        [Fact]
        public void GetAll_ShouldReturnAllLines()
        {
            _context.SalesOrderLines.AddRange(
                new SalesOrderLine { Quantity = 1, Amount = 10 },
                new SalesOrderLine { Quantity = 2, Amount = 20 }
            );
            _context.SaveChanges();

            var result = _service.GetAll().ToList();

            result.Count.Should().Be(2);
        }

        [Fact]
        public void GetAll_ShouldReturnEmpty_WhenNoLines()
        {
            var result = _service.GetAll().ToList();
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAllForSalesOrder_ShouldReturnMatchingLines()
        {
            int headerId = 100;

            _context.SalesOrderLines.AddRange(
                new SalesOrderLine { Quantity = 1, Amount = 10, SalesOrderHeaderId = headerId, Item = new Item { Name = "Item1" } },
                new SalesOrderLine { Quantity = 2, Amount = 20, SalesOrderHeaderId = 999 }
            );
            _context.SaveChanges();

            var result = _service.GetAllForSalesOrder(headerId).ToList();

            result.Should().HaveCount(1);
            result[0].SalesOrderHeaderId.Should().Be(headerId);
            result[0].Item.Should().NotBeNull();
        }

        [Fact]
        public void GetAllForSalesOrder_ShouldReturnEmpty_WhenNoMatch()
        {
            var result = _service.GetAllForSalesOrder(12345);
            result.Should().BeEmpty();
        }

        [Fact]
        public void Update_ShouldSetModifiedState()
        {
            var line = new SalesOrderLine { Quantity = 1, Amount = 10 };
            _context.SalesOrderLines.Add(line);
            _context.SaveChanges();

            line.Quantity = 5;
            _service.Update(line);

            _context.Entry(line).State.Should().Be(EntityState.Modified);
        }

        [Fact]
        public void SaveChanges_ShouldReturnTrue_WhenChangesSaved()
        {
            var line = new SalesOrderLine { Quantity = 1, Amount = 10 };
            _context.SalesOrderLines.Add(line);
            var result = _service.SaveChanges();

            result.Should().BeTrue();
        }

        [Fact]
        public void UpdateLineAmount_ShouldReturnFalse_WhenFails()
        {
            var disposedContext = TestDbContextFactory.CreateInMemoryDbContext();
            disposedContext.Dispose();

            var faultyService = new SalesOrderLineService(disposedContext);

            var result = faultyService.UpdateLineAmount(1);

            result.Should().BeFalse();
        }
    }
}
