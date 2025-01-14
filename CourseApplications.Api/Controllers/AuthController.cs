﻿using CourseApplications.Api.Models.Users;
using CourseApplications.DAL;
using CourseApplications.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApplications.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Controller mocked data
        private static readonly UserModel[] users =
        {
           new UserModel {
                UserId = 1,
                Email= "admin@university.ac.za",
                FullName = "System Admin",
                Role = "Admin"
           },
            new UserModel {
                UserId = 2,
                Email= "clerk@university.ac.za",
                FullName = "System Clerk",
                Role = "Staff"
           },
              new UserModel {
                UserId = 3,
                Email= "agent@recruitingwebsite.com",
                FullName = "System Clerk",
                Role = "Client"
           },
        };

        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public AuthController(ILogger<AuthController> logger, IAuthService authService, IUserService userService)
        {
            _logger = logger;
            _authService = authService;
            _userService = userService;
        }

        [Authorize(Roles = RoleConstants.Admin)]
        [HttpGet("users")]
        public IActionResult Get()
        {
            return Ok(_userService.GetAll());
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            // TODO Service must authenticate and provide a Bearer Token
            if (model.Email == null) return BadRequest();

            var validUser = _authService.Authenticate(model.Email);

            if (validUser == null)
            {
                _logger.LogError($"User with username: {model.Email} Not Found at : {DateTime.Now}");
                return NotFound(new { Message = "Incorrect Username/Password, Try Again" });
            }

            _logger.LogInformation($"User authenticated successfully for : {validUser.User.Email}");

            if (validUser != null && model.IsInternal)
            {
                var _message = "Welcome Staff member";
                if (validUser.User.Role == "Admin")
                    _message = "Welcome Administrator";
                return Ok(new
                {
                    Message = _message,
                    validUser
                });
            }

            return Ok(new
            {
                Message = "Welcome Recruiter",
                validUser
            });
        }

    }
}
