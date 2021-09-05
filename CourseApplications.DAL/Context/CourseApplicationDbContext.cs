using CourseApplications.DAL.Enteties;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.DAL.Context
{
    public class CourseApplicationDbContext : DbContext
    {
        // This api service will be using the code first approach to design 
        // this will allow the api service to be scalable and evolve as we discover new known unknowns.
        public CourseApplicationDbContext(DbContextOptions<CourseApplicationDbContext> options) : base(options) { }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Application>(application =>
            {
                application.HasKey(a => a.ApplicationId);
                application.Property(a => a.FullName).IsRequired();
                application.Property(a => a.Email).IsRequired();
                application.Property(a => a.PhoneNumber).IsRequired();
                application.Property(a => a.HighestGradePassed).IsRequired();
                application.HasOne(a => a.Course).WithMany(c => c.Applications);
            });

            modelBuilder.Entity<Course>(course =>
            {
                course.HasKey(c => c.CourseId);
                course.Property(c => c.Name).IsRequired();
                course.Property(c => c.Faculty).IsRequired();
                course.Property(c => c.Department).IsRequired();
                course.HasMany(c => c.Applications).WithOne(a => a.Course);
            });

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(u => u.UserId);
                user.Property(u => u.Email).IsRequired();
                user.Property(u => u.Role).IsRequired();
            });
        }
    }
}
