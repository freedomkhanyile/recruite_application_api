using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.Api.Models.Courses
{
    public class CourseModel
    {
        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Term { get; set; }

        public string Faculty { get; set; }

        public string Department { get; set; }
    }
}
