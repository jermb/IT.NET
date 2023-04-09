using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    internal class InvalidCourseNumberException : Exception
    {
        public InvalidCourseNumberException(string message) : base(message)
        {

        }
    }
}
