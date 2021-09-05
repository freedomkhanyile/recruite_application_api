using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Api.Models.Users
{
    public class AuthenticateModel
    {
        public string Email { get; set; }
        public bool IsInternal { get; set; }
    }
}
