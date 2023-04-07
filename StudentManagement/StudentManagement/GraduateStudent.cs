using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    internal class GraduateStudent : Student
    {

        public GraduateStudent(string first, string last, int gender, int age, string id) : base(first, last, gender, age, id)
        {
            base.MinCourseNum = 5000;
            base.MaxCourseNum = 9999;
        }

    }
}
