using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using OMSAPI.Models;
using OMSAPI.Services;
using Xunit;

namespace DotTestKit.UnitTests.Services
{
    public class ItemServiceTests
    {
        private DbContextOptions<OMSDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<OMSDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Unique DB per test
                .Options;
        }

        [Fact]
        public void Create_ShouldAddItem()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new ItemService(context);
                var item = new Item { Id = 1, Name = "TestItem" };

                service.Create(item);
                service.SaveChanges();

                var result = context.Items.FirstOrDefault(i => i.Id == 1);
                Assert.NotNull(result);
                Assert.Equal("TestItem", result.Name);
            }
        }

        [Fact]
        public void Create_ShouldThrowArgumentNullException_WhenItemIsNull()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new ItemService(context);
                Assert.Throws<ArgumentNullException>(() => service.Create(null));
            }
        }

        [Fact]
        public void Get_ShouldReturnCorrectItem()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                context.Items.Add(new Item { Id = 2, Name = "GetItem" });
                context.SaveChanges();

                var service = new ItemService(context);
                var result = service.Get(2);

                Assert.NotNull(result);
                Assert.Equal("GetItem", result.Name);
            }
        }

        [Fact]
        public void GetAll_ShouldReturnAllItems()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                context.Items.AddRange(
                    new Item { Id = 3, Name = "Item1" },
                    new Item { Id = 4, Name = "Item2" }
                );
                context.SaveChanges();

                var service = new ItemService(context);
                var result = service.GetAll();

                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public void Delete_ShouldRemoveItem()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var item = new Item { Id = 5, Name = "ToDelete" };
                context.Items.Add(item);
                context.SaveChanges();

                var service = new ItemService(context);
                service.Delete(item);
                service.SaveChanges();

                var result = context.Items.FirstOrDefault(i => i.Id == 5);
                Assert.Null(result);
            }
        }

        [Fact]
        public void Delete_ShouldThrowArgumentNullException_WhenItemIsNull()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new ItemService(context);
                Assert.Throws<ArgumentNullException>(() => service.Delete(null));
            }
        }

        [Fact]
        public void Update_ShouldSetEntityStateToModified()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var item = new Item { Id = 6, Name = "OldName" };
                context.Items.Add(item);
                context.SaveChanges();

                item.Name = "NewName";
                var service = new ItemService(context);
                service.Update(item);
                service.SaveChanges();

                var updated = context.Items.Find(6);
                Assert.Equal("NewName", updated.Name);
            }
        }

        [Fact]
        public void SaveChanges_ShouldReturnTrue()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                context.Items.Add(new Item { Id = 7, Name = "SaveTest" });
                var service = new ItemService(context);

                var result = service.SaveChanges();
                Assert.True(result);
            }
        }
    }
}
