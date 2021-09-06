using CourseApplications.Api.Security;
using CourseApplications.DAL.Entities;
using CourseApplications.DAL.UnitOfWork;
using CourseApplications.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IUserService _userService;
        public AuthService(IUnitOfWork unitOfWork, ITokenBuilder tokenBuilder, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _tokenBuilder = tokenBuilder;
            _userService = userService;
        }

        public UserToken Authenticate(string email)
        {
            // get user by id
            var user = _userService.GetByEmail(email);

            if (user == null)
                throw new Exception("User not found");

            // create token
            var expiryPeriod = DateTime.Now.ToLocalTime() + TokenAuthOptions.ExpiresSpan;
            var roles = new List<string>() { user.Role };
            var token = _tokenBuilder.Build(user.Email, roles.ToArray(), expiryPeriod);

            return new UserToken
            {
                ExpiresAt = expiryPeriod,
                Token = token,
                User = user
            };
        }
    }
}
