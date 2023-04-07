using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    internal class Course : IComparable<Course>
    {

        public string Name { get; }
        public string Prefix { get; }
        public int Number { get; }
        public int CreditHours { get; }
        public double Grade { get; }
        public string Display { get => $"{Prefix} {Number}"; }

        public Course(string name, string prefix, int number, int hours, double grade)
        {

        }

        public int CompareTo(Course? other)
        {
            int prefixCompare = Prefix.CompareTo(other.Prefix);

            if (prefixCompare == -1) return -1;
            else if (prefixCompare == 1) return 1;
            else if (prefixCompare == 0)
            {
                return Number.CompareTo(other.Number);
            }
            else return 0;
        }
    }
}
