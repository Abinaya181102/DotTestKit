using Microsoft.EntityFrameworkCore;
using OMSAPI.DataContext;
using System;

namespace OMSAPI.UnitTests.TestHelpers
{
    public static class TestDbContextFactory
    {
        public static OMSDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OMSDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new OMSDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}
