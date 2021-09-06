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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<User> GetAll()
        {
            return _uow.Query<User>()
                .Where(u => u.Email != null);
        }

        public User GetByEmail(string email)
        {
            var user = GetAll().FirstOrDefault(u => u.Email == email);
            if (user == null)
                throw new Exception("user not found");
            return user;
        }

        public User GetByUserId(int id)
        {
            var user = GetAll().FirstOrDefault(u => u.UserId == id);
            if (user == null)
                throw new Exception("user not found");
            return user;
        }
    }
}
