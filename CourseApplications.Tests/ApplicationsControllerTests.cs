using CourseApplications.Api;
using CourseApplications.Api.Models.Applications;
using CourseApplications.DAL.Context;
using CourseApplications.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace CourseApplications.Tests
{
    [Collection("Integration Tests")]
    public class ApplicationsControllerTests : IClassFixture<ApplicationsControllerTests.DbSetup>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private Application _mockApplication;

        // provides db seeding before all tests are run, and then cleans up after.
        [Collection("Integration Tests")]
        public class DbSetup : IDisposable
        {
            private CourseApplicationDbContext _dbContext;

            public DbSetup(WebApplicationFactory<Startup> factory)
            {
                // returns a single lifetime instance of our context used by controller classes.

                _dbContext = factory.Services.GetRequiredService<CourseApplicationDbContext>();

                // seed database
                var course = new Course
                {
                    CourseId = 1,
                    Name = "Information Technology",
                    Faculty = "Science and Mathamatics",
                    Department = "Account and Informatics",
                    Term = "Y1-S1",
                };

                _dbContext.Courses.Add(course);

                var app1 = new Application
                {
                    ApplicationId = 1,
                    FullName = "Tim Carey",
                    Address = "Tom baker street",
                    Gender = "Male",
                    Email = "tim@hotmail.com",
                    PhoneNumber = "074421215",
                    HighestGradePassed = "Grade 11",
                    DateOfBirth = "2001 July 30",
                    ApplicationDate = DateTime.Now,
                    Status = "Pending",
                    Course = course
                };

                _dbContext.Applications.Add(app1);

                var app2 = new Application
                {
                    ApplicationId = 2,
                    FullName = "Tammy Duncins",
                    Address = "New Road Ave",
                    Gender = "Female",
                    Email = "Tammy@gmail.com.com",
                    PhoneNumber = "0741215451",
                    HighestGradePassed = "Grade 12",
                    DateOfBirth = "2000 September 10",
                    ApplicationDate = DateTime.Now,
                    Status = "Accepted",
                    Course = course
                };

                _dbContext.Applications.Add(app2);

                _dbContext.SaveChanges();
            }

            public void Dispose()
            {

                var applications = _dbContext.Applications.ToArray();
                _dbContext.Applications.RemoveRange(applications);

                var courses = _dbContext.Courses.ToArray();
                _dbContext.Courses.RemoveRange(courses);

                _dbContext.SaveChanges();

            }
        }

        public ApplicationsControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Get_ShouldReturn_AllApplications()
        {
            // Arrange 
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/Applications");
            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            var responseApplications = JsonSerializer
                .Deserialize<IEnumerable<ApplicationModel>>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            Assert.NotNull(responseApplications);
            Assert.Equal(2, responseApplications.Count());
            Assert.Contains(responseApplications, app => app.ApplicationId == 1);
            Assert.Contains(responseApplications, app => app.ApplicationId == 2);
        }

        [Fact]
        public async Task GetById_ShoulReturn_ApplicationById()
        {
            // Arrange 
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/applications/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            var responseObj = JsonSerializer
                .Deserialize<ApplicationModel>(
                await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            Assert.Equal(1, responseObj.ApplicationId);
            Assert.Equal("Pending", responseObj.Status);
            Assert.Equal(1, responseObj.CourseId);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_InvalidApplicationId()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act 
            var response = await client.GetAsync("/api/applications/123");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateApplication_ShouldReturn_Success()
        {
            // Arrange
            var client = _factory.CreateClient();

            var applicationModel = new ApplicationModel
            {
                ApplicationId = 3,
                FullName = "Peter Parker",
                Address = "5th Aven",
                Gender = "Male",
                Email = "peterp@gmail.com.com",
                PhoneNumber = "07451215487",
                HighestGradePassed = "Grade 12",
                DateOfBirth = "1999 September 10",
                ApplicationDate = DateTime.Now,
                Status = "Pending",
                CourseId = 1
            };

            // serialize our request
            var content = new StringContent(JsonSerializer.Serialize(applicationModel,
                 new JsonSerializerOptions { IgnoreNullValues = true }), Encoding.UTF8, "application/json");

            try
            {
                // Act 
                var response = await client.PostAsync("/api/applications", content);

                // Assert
                response.EnsureSuccessStatusCode();
                Assert.NotNull(response.Content);

                var responseObj = JsonSerializer
                .Deserialize<ApplicationModel>(
                await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

                Assert.NotNull(responseObj);
                Assert.Equal(applicationModel.ApplicationId, responseObj.ApplicationId);
                Assert.Equal(applicationModel.FullName, responseObj.FullName);
                Assert.Equal(applicationModel.Email, responseObj.Email);

            }
            finally
            {
                // clean up
                var dbContext = _factory.Services.GetRequiredService<CourseApplicationDbContext>();
                var applicationEntity = await dbContext.Applications.FindAsync(applicationModel.ApplicationId);
                dbContext.Applications.Remove(applicationEntity);
                await dbContext.SaveChangesAsync();
            }

        }

        [Fact]
        public async Task Update_ShoulReturnSuccess_ApplicationUpdate()
        {
            // Arrange
            var client = _factory.CreateClient();
            var applicationToUpdate = new ApplicationModel
            {
                ApplicationId = 2,
                FullName = "Tammy Duncins",
                Address = "New Road Ave",
                Gender = "Female",
                Email = "Tammy@gmail.com.com",
                PhoneNumber = "0741215451",
                HighestGradePassed = "Grade 12",
                DateOfBirth = "2000 September 10",
                ApplicationDate = DateTime.Now,
                Status = "Rejected"
            };

            // serialize our request
            var content = new StringContent(JsonSerializer.Serialize(applicationToUpdate,
                 new JsonSerializerOptions { IgnoreNullValues = true }), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PutAsync("/api/applications/2", content);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnSuccessAndDeletedApplication()
        {
            // Arrange
            var client = _factory.CreateClient();
            var dbContext = _factory.Services.GetRequiredService<CourseApplicationDbContext>();
            var courseEntity = new Course
            {
                CourseId = 2,
                Name = "Information Technology",
                Faculty = "Science and Mathamatics",
                Department = "Account and Informatics",
                Term = "Y1-S1",
            };

            dbContext.Courses.Add(courseEntity);

            var applicationEntity = new Application
            {
                ApplicationId = 3,
                FullName = "Tammy Duncins",
                Address = "New Road Ave",
                Gender = "Female",
                Email = "Tammy@gmail.com.com",
                PhoneNumber = "0741215451",
                HighestGradePassed = "Grade 12",
                DateOfBirth = "2000 September 10",
                ApplicationDate = DateTime.Now,
                Status = "Pending",
                Course = courseEntity
            };

            dbContext.Applications.Add(applicationEntity);
            await dbContext.SaveChangesAsync();


            // Act
            var response = await client.DeleteAsync("/api/applications/3");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            var responseObj = JsonSerializer
               .Deserialize<ApplicationModel>(
               await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            Assert.NotNull(responseObj);

            Assert.Equal(applicationEntity.ApplicationId, responseObj.ApplicationId);
        }
    }
}
