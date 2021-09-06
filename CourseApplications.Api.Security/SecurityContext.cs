using CourseApplications.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CourseApplications.DAL.UnitOfWork;
using CourseApplications.DAL;

namespace CourseApplications.Api.Security
{
    public class SecurityContext : ISecurityContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        private User _user;

        public SecurityContext(IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
        {
            _contextAccessor = contextAccessor;
            _unitOfWork = unitOfWork;
        }

        public User User
        {
            get
            {
                if (_user != null) return _user;
                if (!_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
                    throw new UnauthorizedAccessException();

                var email = _contextAccessor.HttpContext.User.Identity.Name;

                _user = _unitOfWork.Query<User>()
                    .Where(x => x.Email == email)
                    .FirstOrDefault();

                if (_user == null)
                    throw new UnauthorizedAccessException("userEntity not found");

                return _user;
            }
        }
        public bool IsAdministrator { get { return User.Role == RoleConstants.Admin; } }
    }
}
