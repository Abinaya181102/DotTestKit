using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using Xunit;

namespace DotTestKit.UnitTests.Services
{
    public class SalesOrderLineServiceTests
    {
        private DbContextOptions<OMSDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<OMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void GetAll_ShouldReturnAllSalesOrderLines()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                context.SalesOrderLines.AddRange(
                    new SalesOrderLine { Id = 1, SalesOrderHeaderId = 1 },
                    new SalesOrderLine { Id = 2, SalesOrderHeaderId = 1 }
                );
                context.SaveChanges();

                var service = new SalesOrderLineService(context);
                var result = service.GetAll();

                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public void Get_ShouldReturnCorrectSalesOrderLine()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                // Add item first because SalesOrderLine has a navigation to Item
                var item = new Item { Id = 1, Name = "Test Item" };
                context.Items.Add(item);

                var line = new SalesOrderLine
                {
                    Id = 1,
                    SalesOrderHeaderId = 1,
                    ItemId = 1,
                    Item = item // establish relationship
                };

                context.SalesOrderLines.Add(line);
                context.SaveChanges();

                var service = new SalesOrderLineService(context);
                var result = service.Get(1);

                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.NotNull(result.Item);
                Assert.Equal("Test Item", result.Item.Name);
            }
        }


        [Fact]
        public void Create_ShouldAddSalesOrderLine()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                var service = new SalesOrderLineService(context);
                var newLine = new SalesOrderLine { Id = 1, SalesOrderHeaderId = 1 };
                service.Create(newLine);
                service.SaveChanges();

                Assert.Equal(1, context.SalesOrderLines.Count());
            }
        }

        [Fact]
        public void Delete_ShouldRemoveSalesOrderLine()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                var line = new SalesOrderLine { Id = 1, SalesOrderHeaderId = 1 };
                context.SalesOrderLines.Add(line);
                context.SaveChanges();

                var service = new SalesOrderLineService(context);
                service.Delete(line);
                service.SaveChanges();

                Assert.Empty(context.SalesOrderLines);
            }
        }

        [Fact]
        public void GetAllForSalesOrder_ShouldReturnFilteredLines()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                // Add item that will be used in the lines
                var item = new Item { Id = 1, Name = "Test Item" };
                context.Items.Add(item);

                context.SalesOrderLines.AddRange(
                    new SalesOrderLine { Id = 1, SalesOrderHeaderId = 1, ItemId = 1, Item = item },
                    new SalesOrderLine { Id = 2, SalesOrderHeaderId = 2, ItemId = 1, Item = item }
                );
                context.SaveChanges();

                var service = new SalesOrderLineService(context);
                var result = service.GetAllForSalesOrder(1);

                Assert.Single(result);
                Assert.Equal(1, result.First().SalesOrderHeaderId);
            }
        }


        [Fact]
        public void Update_ShouldModifySalesOrderLine()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                var line = new SalesOrderLine { Id = 1, SalesOrderHeaderId = 1 };
                context.SalesOrderLines.Add(line);
                context.SaveChanges();

                line.SalesOrderHeaderId = 2;
                var service = new SalesOrderLineService(context);
                service.Update(line);
                service.SaveChanges();

                Assert.Equal(2, context.SalesOrderLines.First().SalesOrderHeaderId);
            }
        }
    }
}
