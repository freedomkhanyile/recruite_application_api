using CourseApplications.Api.Models.Applications;
using CourseApplications.DAL.Context;
using CourseApplications.DAL.Entities;
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
        /// <summary>
        /// Get All Applications in the system
        /// </summary>
        /// <returns>List of Application Model</returns>
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
        /// <returns>Object of Application Model</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApplicationModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var applicationObj = await _dbContext.Applications.FindAsync(id);

            if (applicationObj == null) return NotFound();

            return Ok(applicationObj.ToApiModel());
        }

        /// <summary>
        ///  Create a application
        /// </summary>
        /// <param name="ApplicationModel"></param>
        /// <returns>Saved Application</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApplicationModel>> Create([FromBody] ApplicationModel model)
        {
            // required fields here for validating the incoming model from client.
            if (string.IsNullOrEmpty(model.FullName)
                || string.IsNullOrEmpty(model.Email)
                || string.IsNullOrEmpty(model.PhoneNumber)
                || string.IsNullOrEmpty(model.HighestGradePassed)
                )
                return BadRequest();

            var _course = await _dbContext.Courses.FindAsync(model.CourseId);

            if (_course == null)
                return NotFound(new { message = $"Course with id : {model.CourseId} not found" });

            var existingApplication = await _dbContext.Applications
                .FirstOrDefaultAsync(x => x.Email == model.Email 
                && x.Course.CourseId == model.CourseId);

            if (existingApplication != null) 
                return Conflict(new { message = $"Application already exists, please update" });

            var appToAdd = model.ToEntity(_course);

            _dbContext.Applications.Add(appToAdd);

            await _dbContext.SaveChangesAsync();

            var updatedAppModel = appToAdd.ToApiModel();

            return CreatedAtAction(nameof(Get), new { id = model.ApplicationId }, updatedAppModel);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ApplicationModel model)
        {
            if(model.ApplicationId != id 
                || string.IsNullOrEmpty(model.FullName) 
                || string.IsNullOrEmpty(model.Email)
                || string.IsNullOrEmpty(model.PhoneNumber)
                || string.IsNullOrEmpty(model.HighestGradePassed)
                )
                return BadRequest();

            var applicationEntity = await _dbContext.Applications.FindAsync(id);

            if (applicationEntity == null)
                return NotFound(new { message = $"Application with id : {id} not found" });

            // get the course if we have a new update,
            // else return course from db on initial create.
            var _course = model.CourseId == applicationEntity.Course.CourseId
                ? await _dbContext.Courses.FindAsync(model.CourseId) : applicationEntity.Course;

            if(_course == null)
                return NotFound(new { message = $"Course with id : {model.CourseId} not found" });

            applicationEntity.ToUpdateEntity(model, _course);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApplicationModel>> Delete(int id)
        {
            var applicationEntity = await _dbContext.Applications.FindAsync(id);
            if (applicationEntity == null)
                return NotFound(new { message = $"Application with id : {id} not found" });

            _dbContext.Applications.Remove(applicationEntity);
            await _dbContext.SaveChangesAsync();
            return Ok(applicationEntity.ToApiModel());
        }


    }
}
