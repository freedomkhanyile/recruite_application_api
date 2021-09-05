using CourseApplications.Api.Models.Applications;
using CourseApplications.DAL.Enteties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApplications.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ILogger<ApplicationsController> _logger;

        public ApplicationsController(ILogger<ApplicationsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var applicationObj = new Application
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
                Course = new Course
                {
                    CourseId = 1,
                    Name = "Information Technology",
                    Faculty = "Science and Mathamatics",
                    Department = "Account and Informatics",
                    Term = "Y1-S1",
                }
            };
            return Ok(applicationObj.ToApiModel());
        }
    }
}
