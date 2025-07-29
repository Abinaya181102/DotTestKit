using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using Xunit;

namespace OMSAPI.Tests
{
    public class AddressServiceTests
    {
        private DbContextOptions<OMSDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<OMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB per test
                .Options;
        }

        [Fact]
        public void Create_ShouldAddAddress()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                var service = new AddressService(context);
                var address = new Address { Country = "India", PostCode = "600001", Street = "Main St", BuildingNo = "101", CustomerId = 1 };

                service.Create(address);
                service.SaveChanges();

                Assert.Equal(1, context.Addresses.Count());
                Assert.Equal("India", context.Addresses.First().Country);
            }
        }

        [Fact]
        public void Get_ShouldReturnAddress()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                var address = new Address { Id = 1, Country = "India", PostCode = "600001", Street = "Main St", BuildingNo = "101", CustomerId = 1 };
                context.Addresses.Add(address);
                context.SaveChanges();

                var service = new AddressService(context);
                var result = service.Get(1);

                Assert.NotNull(result);
                Assert.Equal("India", result.Country);
            }
        }

        [Fact]
        public void GetAll_ShouldReturnAllAddresses()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                context.Addresses.AddRange(
                    new Address { Country = "India", PostCode = "600001", Street = "Main St", BuildingNo = "101", CustomerId = 1 },
                    new Address { Country = "USA", PostCode = "10001", Street = "Second St", BuildingNo = "102", CustomerId = 2 }
                );
                context.SaveChanges();

                var service = new AddressService(context);
                var results = service.GetAll();

                Assert.Equal(2, results.Count());
            }
        }

        [Fact]
        public void GetAllForCustomer_ShouldReturnFilteredAddresses()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                context.Addresses.AddRange(
                    new Address { Country = "India", PostCode = "600001", Street = "Main St", BuildingNo = "101", CustomerId = 1 },
                    new Address { Country = "USA", PostCode = "10001", Street = "Second St", BuildingNo = "102", CustomerId = 2 }
                );
                context.SaveChanges();

                var service = new AddressService(context);
                var results = service.GetAllForCustomer(1).ToList();

                Assert.Single(results);
                Assert.Equal("India", results[0].Country);
            }
        }


        [Fact]
        public void Update_ShouldModifyAddress()
        {
            var options = GetInMemoryOptions();
            using (var context = new OMSDbContext(options))
            {
                var address = new Address { Id = 1, Country = "India", PostCode = "600001", Street = "Main St", BuildingNo = "101", CustomerId = 1 };
                context.Addresses.Add(address);
                context.SaveChanges();

                var service = new AddressService(context);
                address.City = "Chennai";
                service.Update(address);
                service.SaveChanges();

                var updated = context.Addresses.Find(1);
                Assert.Equal("Chennai", updated.City);
            }
        }
    }
}
