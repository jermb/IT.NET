using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    internal class InvalidCourseNumberException : Exception
    {
        public int Max { get; }
        public int Min { get; }
        public InvalidCourseNumberException(int max, int min) : base($"Course number must be between {min} and {max}.")
        {
            Max = max;
            Min = min;
        }
    }
}
