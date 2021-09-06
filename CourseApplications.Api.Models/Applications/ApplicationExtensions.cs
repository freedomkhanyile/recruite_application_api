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
                CourseId = application.Course != null ? application.Course.CourseId : 0,
            };        
        }

        public static Application ToEntity(this ApplicationModel model, Course course)
        {
            if (course.CourseId != model.CourseId) throw new NotSupportedException();

            return new Application
            {
                ApplicationId = model.ApplicationId,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                HighestGradePassed = model.HighestGradePassed,
                ApplicationDate = model.ApplicationDate,
                Address = model.Address,
                Status = model.Status,
                Course = course
            };
        }
        public static void ToUpdateEntity(this Application applicationToUpdate, ApplicationModel model, Course course)
        {
            if (applicationToUpdate.ApplicationId != model.ApplicationId) throw new NotSupportedException();

            applicationToUpdate.FullName = model.FullName;
            applicationToUpdate.Email = model.Email;
            applicationToUpdate.PhoneNumber = model.PhoneNumber;
            applicationToUpdate.Gender = model.Gender;
            applicationToUpdate.DateOfBirth = model.DateOfBirth;
            applicationToUpdate.HighestGradePassed = model.HighestGradePassed;
            applicationToUpdate.ApplicationDate = model.ApplicationDate;
            applicationToUpdate.Address = model.Address;
            applicationToUpdate.Status = model.Status;
            applicationToUpdate.Course = course;
         
        }
    }
}
