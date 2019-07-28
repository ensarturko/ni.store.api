using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Ni.Store.Api.Data;
using Xunit;

namespace Ni.Store.Api.Tests
{
    public partial class StoresControllerTests 
    {
        [Fact]
        internal async Task Get_Should_Return_Existing_Record()
        {
            // Arrange
            var client = _factory.CreateClient();

            Data.Entities.Store store;

            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

                store = new Data.Entities.Store
                {
                    Key = "TestKey1",
                    Value = "TestValue1",
                    ExpirationTime = DateTime.Now
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = $"/api/keys/{store.Id}";

            // Act
            var response = await client.GetAsync(requestUri);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(responseString);

            var jObject = JObject.Parse(responseString);

            Assert.Equal(9, jObject.Count);

            Assert.True(jObject.ContainsKey("id"));
            Assert.NotEqual(0, jObject["id"].Value<int>());
        }
    }
}
