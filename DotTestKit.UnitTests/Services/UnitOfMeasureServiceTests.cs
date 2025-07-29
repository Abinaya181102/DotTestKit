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
    public class UnitOfMeasureServiceTests
    {
        private DbContextOptions<OMSDbContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<OMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public void GetAll_ShouldReturnAllUnits()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                context.UnitsOfMeasure.AddRange(
                    new UnitOfMeasure { Code = "KG", Name = "Kilogram" },
                    new UnitOfMeasure { Code = "L", Name = "Liter" }
                );
                context.SaveChanges();

                var service = new UnitOfMeasureService(context);
                var result = service.GetAll();

                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public void Get_ShouldReturnCorrectUnit()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                context.UnitsOfMeasure.Add(new UnitOfMeasure { Code = "KG", Name = "Kilogram" });
                context.SaveChanges();

                var service = new UnitOfMeasureService(context);
                var unit = service.Get("KG");

                Assert.NotNull(unit);
                Assert.Equal("KG", unit.Code);
            }
        }

        [Fact]
        public void Create_ShouldAddNewUnit()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new UnitOfMeasureService(context);
                var newUnit = new UnitOfMeasure { Code = "BOX", Name = "Box" };

                service.Create(newUnit);
                service.SaveChanges();

                Assert.Single(context.UnitsOfMeasure);
                Assert.Equal("BOX", context.UnitsOfMeasure.First().Code);
            }
        }

        [Fact]
        public void Update_ShouldModifyExistingUnit()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var unit = new UnitOfMeasure { Code = "CTN", Name = "Carton" };
                context.UnitsOfMeasure.Add(unit);
                context.SaveChanges();

                unit.Name = "Updated Carton";
                var service = new UnitOfMeasureService(context);
                service.Update(unit);
                service.SaveChanges();

                Assert.Equal("Updated Carton", context.UnitsOfMeasure.First().Name);
            }
        }

        [Fact]
        public void Delete_ShouldRemoveUnit()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var unit = new UnitOfMeasure { Code = "ML", Name = "Milliliter" };
                context.UnitsOfMeasure.Add(unit);
                context.SaveChanges();

                var service = new UnitOfMeasureService(context);
                service.Delete(unit);
                service.SaveChanges();

                Assert.Empty(context.UnitsOfMeasure);
            }
        }

        [Fact]
        public void Create_ShouldThrowException_WhenNull()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new UnitOfMeasureService(context);
                Assert.Throws<ArgumentNullException>(() => service.Create(null));
            }
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenNull()
        {
            var options = GetInMemoryOptions();

            using (var context = new OMSDbContext(options))
            {
                var service = new UnitOfMeasureService(context);
                Assert.Throws<ArgumentNullException>(() => service.Delete(null));
            }
        }
    }
}
