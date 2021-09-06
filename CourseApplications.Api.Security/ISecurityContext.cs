using CourseApplications.DAL.Entities;
using System;

namespace CourseApplications.Api.Security
{
    public interface ISecurityContext
    {
        User User { get; }

        bool IsAdministrator { get; }
    }
}
