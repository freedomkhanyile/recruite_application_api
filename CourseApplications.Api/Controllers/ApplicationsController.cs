using CourseApplications.Api.Models.Applications;
using CourseApplications.DAL.Context;
using CourseApplications.DAL.Enteties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly CourseApplicationDbContext _dbContext;
        public ApplicationsController(ILogger<ApplicationsController> logger, CourseApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<ApplicationModel>>> Get()
        {
            var applications = await _dbContext.Applications.ToArrayAsync();
            return Ok(applications.Select(a => a.ToApiModel()));
        }

        /// <summary>
        ///  Get Application by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Application Model</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationModel), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(statusCode:StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
           var applicationObj = await _dbContext.Applications.FindAsync(id);

            if (applicationObj == null) return NotFound();

            return Ok(applicationObj.ToApiModel());
        }
    }
}
