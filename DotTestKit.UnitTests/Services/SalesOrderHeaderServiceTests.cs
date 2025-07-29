using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using Xunit;

namespace DotTestKit.UnitTests.Services
{
    public class SalesOrderHeaderServiceTests
    {
        private DbContextOptions<OMSDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<OMSDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void Create_ShouldAddSalesOrderHeader()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                var service = new SalesOrderHeaderService(context);
                var header = new SalesOrderHeader { OrderDate = DateTime.Now, ShipmentDate = DateTime.Now };

                service.Create(header);
                service.SaveChanges();

                Assert.Equal(1, context.SalesOrderHeaders.Count());
            }
        }

        [Fact]
        public void Get_ShouldReturnSalesOrderHeader()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                context.SalesOrderHeaders.Add(new SalesOrderHeader { Id = 1 });
                context.SaveChanges();

                var service = new SalesOrderHeaderService(context);
                var result = service.Get(1);

                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        public void Delete_ShouldRemoveSalesOrderHeader()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                var header = new SalesOrderHeader { Id = 1 };
                context.SalesOrderHeaders.Add(header);
                context.SaveChanges();

                var service = new SalesOrderHeaderService(context);
                service.Delete(header);
                service.SaveChanges();

                Assert.Empty(context.SalesOrderHeaders);
            }
        }

        [Fact]
        public void GetAll_ShouldReturnAllSalesOrderHeaders()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                context.SalesOrderHeaders.AddRange(
                    new SalesOrderHeader { Id = 1 },
                    new SalesOrderHeader { Id = 2 }
                );
                context.SaveChanges();

                var service = new SalesOrderHeaderService(context);
                var result = service.GetAll();

                Assert.Equal(2, result.Count());
            }
        }
    }
}