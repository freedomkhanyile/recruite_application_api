using CourseApplications.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Services.Contracts
{
    public interface IUserService
    {
        User GetByUserId(int id);
        User GetByEmail(string email);
        IQueryable<User> GetAll();
    }
}
