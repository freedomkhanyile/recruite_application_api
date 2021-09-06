using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Api.Security
{
    public interface ITokenBuilder
    {
        string Build(string name, string[] roles, DateTime expireDate);
    }
}
