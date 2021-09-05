using CourseApplications.DAL.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Api.Models.Applications
{
    public static class ApplicationExtensions
    {
        public static ApplicationModel ToApiModel(this Application application) {
            return new ApplicationModel
            {
                ApplicationId = application.ApplicationId,
                FullName = application.FullName,
                Email = application.Email,
                PhoneNumber = application.PhoneNumber,
                Gender = application.Gender,
                DateOfBirth = application.DateOfBirth,
                HighestGradePassed = application.HighestGradePassed,
                ApplicationDate = application.ApplicationDate,
                Address = application.Address,
                Status = application.Status,
                CourseId = application.Course.CourseId,
            };
        
        }
    }
}
