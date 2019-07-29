using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
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
        internal async Task Post_Should_Return_Created_Record()
        {
            // Arrange
            var client = _factory.CreateClient();
            const string requestUri = "/api/keys";

            var requestModel = new 
            {
                Key = "TestKey1",
                Value = "TestValue1",
                ExpirationTime = DateTime.Now.AddDays(1)
            };

            // Act
            var response = await client.PostAsJsonAsync(requestUri, requestModel);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();

            Assert.NotEmpty(responseString);

            var jObject = JObject.Parse(responseString);

            Assert.Equal(4, jObject.Count);

            Assert.True(jObject.ContainsKey("id"));
            Assert.NotEqual(0, jObject["id"].Value<int>());

            Assert.True(jObject.ContainsKey("key"));
            Assert.Equal("TestKey1", jObject["key"].Value<string>());

            Assert.True(jObject.ContainsKey("value"));
            Assert.Equal("TestValue1", jObject["value"].Value<string>());

            Assert.True(jObject.ContainsKey("expirationTime"));
        }

        [Fact]
        internal async Task Post_Should_Return_Error_When_Key_Is_Empty()
        {
            // Arrange
            var client = _factory.CreateClient();
            const string requestUri = "/api/keys";

            var requestModel = new
            {
                Key = string.Empty,
                Value = "TestValue1",
                ExpirationTime = DateTime.Now.AddDays(1)
            };

            // Act
            var response = await client.PostAsJsonAsync(requestUri, requestModel);

            // Assert
            var responseModel = await response.Content.ReadAsAsync<string[]>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Single(responseModel);
            Assert.Equal("Key cannot be null or empty.", responseModel[0]);
        }

        [Fact]
        internal async Task Post_Should_Return_Error_When_Value_Is_Empty()
        {
            // Arrange
            var client = _factory.CreateClient();
            const string requestUri = "/api/keys";

            var requestModel = new
            {
                Key = "TestKey1",
                Value = string.Empty,
                ExpirationTime = DateTime.Now.AddDays(1)
            };

            // Act
            var response = await client.PostAsJsonAsync(requestUri, requestModel);

            // Assert
            var responseModel = await response.Content.ReadAsAsync<string[]>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Assert.Single(responseModel);
            Assert.Equal("Value cannot be null or empty.", responseModel[0]);
        }

        [Fact]
        internal async Task Post_Should_Return_Error_When_The_Record_Already_Exists()
        {
            // Arrange
            var client = _factory.CreateClient();

            using (var scope = _factory.Server.Host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

                await dbContext.Stores.AddAsync(
                    new Data.Entities.Store()
                    {
                        Key = "TestKey1",
                        Value = "TestValue1",
                        ExpirationTime = DateTime.Now.AddDays(1)
                    });

                await dbContext.SaveChangesAsync();
            }

            const string requestUri = "/api/keys";

            var requestModel = new
            {
                Key = "TestKey1",
                Value = "TestValue1",
                ExpirationTime = DateTime.Now.AddDays(1)
            };

            // Act
            var response = await client.PostAsJsonAsync(requestUri, requestModel);

            // Assert
            var responseModel = await response.Content.ReadAsAsync<string[]>();
                
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Single(responseModel);
            Assert.Equal("This record already exists.", responseModel[0]);
        }
    }
}
