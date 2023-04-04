using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    internal class GraduateStudent : Student
    {

        public GraduateStudent()
        {
            base.MinCourseNum = 5000;
            base.MaxCourseNum = 9999;
        }

    }
}
