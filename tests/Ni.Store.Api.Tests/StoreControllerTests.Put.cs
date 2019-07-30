using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Ni.Store.Api.Data;
using Xunit;

namespace Ni.Store.Api.Tests
{
    public partial class StoreControllerTests
    {
        [Fact]
        internal async Task Put_Should_Return_Updated_Record()
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
                    ExpirationTime = DateTime.Now.AddDays(1)
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = $"/api/keys/{store.Id}";

            var requestModel = new
            {
                Key = "TestKey2",
                Value = "TestValue2",
                ExpirationTime = DateTime.Now.AddDays(2)
            };

            // Act
            var response = await client.PutAsJsonAsync(requestUri, requestModel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(responseString);

            var jObject = JObject.Parse(responseString);

            Assert.True(jObject.ContainsKey("id"));
            Assert.NotEqual(0, jObject["id"].Value<int>());

            Assert.True(jObject.ContainsKey("key"));
            Assert.Equal("TestKey2", jObject["key"].Value<string>());

            Assert.True(jObject.ContainsKey("value"));
            Assert.Equal("TestValue2", jObject["value"].Value<string>());

            Assert.True(jObject.ContainsKey("expirationTime"));
        }

        [Fact]
        internal async Task Put_Should_Update_Expiration_By_Query()
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
                    ExpirationTime = DateTime.Now.AddDays(1)
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            double? expireIn = 3600;
            var requestUri = $"/api/keys/{store.Id}/{expireIn}";

            var requestModel = new
            {
                Key = "TestKey2",
                Value = "TestValue2",
            };

            // Act
            var response = await client.PutAsJsonAsync(requestUri, requestModel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(responseString);

            var jObject = JObject.Parse(responseString);

            Assert.Equal(DateTime.Now.AddSeconds(3600).Hour, jObject["expirationTime"].Value<DateTime?>().Value.Hour);
            Assert.Equal(DateTime.Now.AddSeconds(3600).Minute, jObject["expirationTime"].Value<DateTime?>().Value.Minute);
            Assert.Equal(DateTime.Now.AddSeconds(3600).Second, jObject["expirationTime"].Value<DateTime?>().Value.Second);
        }

        [Fact]
        internal async Task Put_Should_Return_Error_When_Id_IsNot_Greater_Than_0()
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
                    ExpirationTime = DateTime.Now.AddDays(1)
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = $"/api/keys/0";

            var requestModel = new
            {
                Key = "TestKey2",
                Value = "TestValue2",
                ExpirationTime = DateTime.Now.AddDays(2)
            };

            // Act
            var response = await client.PutAsJsonAsync(requestUri, requestModel);

            // Assert
            var responseModel = await response.Content.ReadAsAsync<string[]>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Single(responseModel);
            Assert.Equal("Id must be greater than zero.", responseModel[0]);
        }

        [Fact]
        internal async Task Put_Should_Return_Error_When_Key_IsEmpty()
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
                    ExpirationTime = DateTime.Now.AddDays(1)
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = $"/api/keys/{store.Id}";

            var requestModel = new
            {
                Key = string.Empty,
                Value = "TestValue2",
                ExpirationTime = DateTime.Now.AddDays(2)
            };

            // Act
            var response = await client.PutAsJsonAsync(requestUri, requestModel);

            // Assert
            var responseModel = await response.Content.ReadAsAsync<string[]>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Single(responseModel);
            Assert.Equal("Key cannot be null or empty.", responseModel[0]);
        }

        [Fact]
        internal async Task Put_Should_Return_Error_When_Value_IsEmpty()
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
                    ExpirationTime = DateTime.Now.AddDays(1)
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = $"/api/keys/{store.Id}";

            var requestModel = new
            {
                Key = "TestKey2",
                Value = string.Empty,
                ExpirationTime = DateTime.Now.AddDays(2)
            };

            // Act
            var response = await client.PutAsJsonAsync(requestUri, requestModel);

            // Assert
            var responseModel = await response.Content.ReadAsAsync<string[]>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Single(responseModel);
            Assert.Equal("Value cannot be null or empty.", responseModel[0]);
        }

        [Fact]
        internal async Task Put_Should_Return_Error_When_Record_Already_Exists()
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
                    ExpirationTime = DateTime.Now.AddDays(1)
                };

                await dbContext.Stores.AddAsync(store);
                await dbContext.SaveChangesAsync();
            }

            var requestUri = $"/api/keys/{store.Id}";

            var requestModel = new
            {
                Key = "TestKey1",
                Value = "TestValue1",
                ExpirationTime = DateTime.Now.AddDays(2)
            };

            // Act
            var response = await client.PutAsJsonAsync(requestUri, requestModel);

            // Assert
            var responseModel = await response.Content.ReadAsAsync<string[]>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Single(responseModel);
            Assert.Equal("This record already exists.", responseModel[0]);
        }

        [Fact]
        internal async Task Put_Should_Return_BadRequest_When_Entity_Does_Not_Exist()
        {
            // Arrange
            var client = _factory.CreateClient();

            const string requestUri = "/api/keys/999";

            var requestModel = new
            {
                Key = "TestKey1",
                Value = "TestValue1",
                ExpirationTime = DateTime.Now.AddDays(2)
            };

            // Act
            var response = await client.PutAsJsonAsync(requestUri, requestModel);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
