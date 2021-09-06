using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseApplications.DAL.Entities
{
    public class Course
    {
        public int CourseId { get; set; }

        public string Name { get; set; }

        public string Term { get; set; }

        public string Faculty { get; set; }

        public string Department { get; set; }

        public ICollection<Application> Applications { get; set; }
    }
}
