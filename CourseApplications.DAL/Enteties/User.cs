using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.DAL.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }

    public class UserToken
    {
        public string Token { get; set; }
        public User User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
