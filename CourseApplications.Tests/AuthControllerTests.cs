using CourseApplications.Api;
using CourseApplications.Api.Models.Users;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CourseApplications.Tests
{
    [Collection("Integration Tests")]
    public class AuthControllerTests
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public AuthControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ReturnsAllUsers()
        {
            // Arrange

            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/Auth/users");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            var responseObj = JsonSerializer
                .Deserialize<List<UserModel>>(
                await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            Assert.Equal(3, responseObj.Count);

        }
    }
}
