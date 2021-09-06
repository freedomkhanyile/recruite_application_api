using CourseApplications.Api;
using CourseApplications.Api.Models.Applications;
using CourseApplications.DAL.Context;
using CourseApplications.DAL.Enteties;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
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
            var response = await client.GetAsync("/api/Applications/1");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(response.Content);

            var responseObj = JsonSerializer
                .Deserialize<ApplicationModel>(
                await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true, });

            Assert.Equal(_mockApplication.ApplicationId, responseObj.ApplicationId);
            Assert.Equal(_mockApplication.Status, responseObj.Status);
            Assert.Equal(_mockApplication.Course.CourseId, responseObj.CourseId);
        }
    }
}
