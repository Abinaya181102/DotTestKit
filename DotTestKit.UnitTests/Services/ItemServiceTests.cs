using FluentAssertions;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using OMSAPI.UnitTests.TestHelpers;
using System;
using System.Linq;
using Xunit;

namespace OMSAPI.UnitTests.Services
{
    public class ItemServiceTests
    {
        private readonly ItemService _service;
        private readonly OMSDbContext _context;

        public ItemServiceTests()
        {
            _context = TestDbContextFactory.CreateInMemoryDbContext();
            _service = new ItemService(_context);
        }

        [Fact]
        public void Create_ShouldAddItem()
        {
            var item = new Item { Name = "Test", UnitOfMeasureCode = "PCS" };
            _service.Create(item);
            _service.SaveChanges();

            _context.Items.Should().Contain(item);
        }

        [Fact]
        public void Get_ShouldReturnItem()
        {
            var item = new Item { Name = "Test", UnitOfMeasureCode = "PCS" };
            _context.Items.Add(item);
            _context.SaveChanges();

            var result = _service.Get(item.Id);

            result.Should().BeEquivalentTo(item);
        }

        [Fact]
        public void Delete_ShouldRemoveItem()
        {
            var item = new Item { Name = "ToDelete", UnitOfMeasureCode = "PCS" };
            _context.Items.Add(item);
            _context.SaveChanges();

            _service.Delete(item);
            _service.SaveChanges();

            _context.Items.Should().NotContain(item);
        }

        [Fact]
        public void GetAll_ShouldReturnAllItems()
        {
            _context.Items.Add(new Item { Name = "Item1", UnitOfMeasureCode = "PCS" });
            _context.Items.Add(new Item { Name = "Item2", UnitOfMeasureCode = "PCS" });
            _context.SaveChanges();

            var result = _service.GetAll();

            result.Count().Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public void Update_ShouldModifyEntityState()
        {
            var item = new Item { Name = "Original", UnitOfMeasureCode = "PCS" };
            _context.Items.Add(item);
            _context.SaveChanges();

            item.Name = "Updated";
            _service.Update(item);
            _service.SaveChanges();

            _context.Items.First().Name.Should().Be("Updated");
        }

        [Fact]
        public void Create_ShouldThrowArgumentNullException_WhenItemIsNull()
        {
            Action act = () => _service.Create(null);
            act.Should().Throw<ArgumentNullException>().WithParameterName("item");
        }

        [Fact]
        public void Delete_ShouldThrowArgumentNullException_WhenItemIsNull()
        {
            Action act = () => _service.Delete(null);
            act.Should().Throw<ArgumentNullException>().WithParameterName("item");
        }
    }
}