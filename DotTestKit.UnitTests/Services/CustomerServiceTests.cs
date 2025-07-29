using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using Xunit;

namespace DotTestKit.UnitTests.Services
{
    public class CustomerServiceTests
    {
        private DbContextOptions<OMSDbContext> GetInMemoryDbOptions()
        {
            return new DbContextOptionsBuilder<OMSDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Use a new DB for each test
                .Options;
        }

        [Fact]
        public void Create_ShouldAddCustomer()
        {
            var options = GetInMemoryDbOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new CustomerService(context);
                var customer = new Customer { Id = 1, Name = "Test Customer" };

                service.Create(customer);
                service.SaveChanges();

                var saved = context.Customers.FirstOrDefault(c => c.Id == 1);
                Assert.NotNull(saved);
                Assert.Equal("Test Customer", saved.Name);
            }
        }

        [Fact]
        public void Create_ShouldThrowArgumentNullException_WhenNull()
        {
            var options = GetInMemoryDbOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new CustomerService(context);
                Assert.Throws<ArgumentNullException>(() => service.Create(null));
            }
        }

        [Fact]
        public void Get_ShouldReturnCorrectCustomer()
        {
            var options = GetInMemoryDbOptions();

            using (var context = new OMSDbContext(options))
            {
                context.Customers.Add(new Customer { Id = 2, Name = "GetTest" });
                context.SaveChanges();

                var service = new CustomerService(context);
                var customer = service.Get(2);

                Assert.NotNull(customer);
                Assert.Equal("GetTest", customer.Name);
            }
        }

        [Fact]
        public void GetAll_ShouldReturnAllCustomers()
        {
            var options = GetInMemoryDbOptions();

            using (var context = new OMSDbContext(options))
            {
                context.Customers.AddRange(
                    new Customer { Id = 3, Name = "C1" },
                    new Customer { Id = 4, Name = "C2" }
                );
                context.SaveChanges();

                var service = new CustomerService(context);
                var result = service.GetAll();

                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public void Delete_ShouldRemoveCustomer()
        {
            var options = GetInMemoryDbOptions();

            using (var context = new OMSDbContext(options))
            {
                var customer = new Customer { Id = 5, Name = "ToDelete" };
                context.Customers.Add(customer);
                context.SaveChanges();

                var service = new CustomerService(context);
                service.Delete(customer);
                service.SaveChanges();

                var result = context.Customers.FirstOrDefault(c => c.Id == 5);
                Assert.Null(result);
            }
        }

        [Fact]
        public void Delete_ShouldThrowArgumentNullException_WhenNull()
        {
            var options = GetInMemoryDbOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new CustomerService(context);
                Assert.Throws<ArgumentNullException>(() => service.Delete(null));
            }
        }

        [Fact]
        public void Update_ShouldSetModifiedState()
        {
            var options = GetInMemoryDbOptions();

            using (var context = new OMSDbContext(options))
            {
                var customer = new Customer { Id = 6, Name = "BeforeUpdate" };
                context.Customers.Add(customer);
                context.SaveChanges();

                customer.Name = "AfterUpdate";

                var service = new CustomerService(context);
                service.Update(customer);
                service.SaveChanges();

                var updated = context.Customers.Find(6);
                Assert.Equal("AfterUpdate", updated.Name);
            }
        }

        [Fact]
        public void SaveChanges_ShouldReturnTrue()
        {
            var options = GetInMemoryDbOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new CustomerService(context);
                context.Customers.Add(new Customer { Id = 7, Name = "SaveCheck" });

                var result = service.SaveChanges();
                Assert.True(result);
            }
        }
    }
}
