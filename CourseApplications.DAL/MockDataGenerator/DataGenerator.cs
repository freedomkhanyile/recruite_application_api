using CourseApplications.DAL.Context;
using CourseApplications.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.DAL.MockDataGenerator
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new CourseApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<CourseApplicationDbContext>>()))
            {

                // look for any user data
                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User
                        {
                            UserId = 1,
                            Email = "admin@university.ac.za",
                            FullName = "System Admin",
                            Role = "Admin"
                        },
                         new User
                         {
                             UserId = 2,
                             Email = "clerk@university.ac.za",
                             FullName = "System Clerk",
                             Role = "Staff"
                         },
                          new User
                          {
                              UserId = 3,
                              Email = "agent@recruitingwebsite.com",
                              FullName = "System Clerk",
                              Role = "Client"
                          }
                        );
                }

                // look for any course data
                if (!context.Courses.Any())
                {
                    context.Courses.AddRange(
                        new Course
                        {
                            CourseId = 1,
                            Name = "Information Technology",
                            Faculty = "Science and Mathamatics",
                            Department = "Account and Informatics",
                            Term = "Y1-S1",
                        },
                    new Course
                    {
                        CourseId = 2,
                        Name = "Business Accounting",
                        Faculty = "Science and Mathamatics",
                        Department = "Account and Informatics",
                        Term = "Y1-S1",
                    });
                }

                // look for any application data
                if (!context.Applications.Any())
                {
                    context.Applications.AddRange(
                        new Application
                        {
                            ApplicationId = 1,
                            FullName = "Tammy Duncins",
                            Address = "New Road Ave",
                            Gender = "Female",
                            Email = "Tammy@gmail.com.com",
                            PhoneNumber = "0741215451",
                            HighestGradePassed = "Grade 12",
                            DateOfBirth = "2000 September 10",
                            ApplicationDate = DateTime.Now,
                            Status = "Accepted",
                        });
                }

                context.SaveChanges();
            }
        }
    }
}
