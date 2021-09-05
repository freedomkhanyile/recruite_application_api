using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Api.Models.Applications
{
    public class ApplicationModel
    {
        public int ApplicationId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string HighestGradePassed { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; }
    }
}
