using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    abstract class Student : Person
    {

        private int id;
        private int minCourseNum;
        private int maxCourseNum;

        protected int MinCourseNum { get => minCourseNum; set => minCourseNum = value; }
        protected int MaxCourseNum { get => maxCourseNum; set => maxCourseNum = value; }

        public bool IsValidCourse(int num)
        {
            return num >= minCourseNum && num <= maxCourseNum;
        }
    }
}
