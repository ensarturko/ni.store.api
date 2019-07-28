using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ni.Store.Api.Data;
using Xunit;

namespace Ni.Store.Api.Tests
{
    public partial class StoresControllerTests :
        IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;

        public StoresControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        private async Task ResetStores()
        {
            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

                dbContext.RemoveRange(await dbContext.Stores.ToListAsync());

                await dbContext.SaveChangesAsync();
            }
        }

        public void Dispose()
        {
            ResetStores().GetAwaiter().GetResult();
        }
    }
}
