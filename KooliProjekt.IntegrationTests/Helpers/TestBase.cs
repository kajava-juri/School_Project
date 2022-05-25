using System;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Testing;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        public WebApplicationFactory<Startup> Factory { get; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<Startup, FakeStartup>();
        }

        public void Dispose()
        {
            var dbContext = (ApplicationDbContext)Factory.Services.GetService(typeof(ApplicationDbContext));
            dbContext.Database.EnsureDeleted();
        }

        // Add you other helper methods here
    }
}