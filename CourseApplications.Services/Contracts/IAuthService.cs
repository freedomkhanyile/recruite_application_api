using CourseApplications.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Services.Contracts
{
    public interface IAuthService
    {
        UserToken Authenticate(string email);         
    }
}
