using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    internal class UndergraduateStudent : Student
    {
       
        public UndergraduateStudent()
        {
            base.MaxCourseNum = 1000;
            base.MinCourseNum = 4999;
        }


    }
}
