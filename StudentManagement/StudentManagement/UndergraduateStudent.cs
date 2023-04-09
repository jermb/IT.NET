using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    internal class UndergraduateStudent : Student
    {
       
        public UndergraduateStudent(string first, string last, int gender, int age, string id) : base(first, last, gender, age, id)
        {
            base.MinCourseNum = 1000;
            base.MaxCourseNum = 4999;
        }
    }
}
