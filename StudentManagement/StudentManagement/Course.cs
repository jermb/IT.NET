using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class Course : IComparable<Course>
    {

        private string name;
        private string prefix;
        public int number;
        public int hours;
        public double grade;

        public string Name { get => name; }
        public string Prefix { get => prefix; }
        public int Number { get => number; }
        public int CreditHours { get => hours; }
        public double Grade { get => grade; }
        public string Display { get => $"{Prefix} {Number}"; }

        public Course(string name, string prefix, int number, int hours, double grade)
        {
            this.name = name;
            this.prefix = prefix;
            this.number = number;
            this.hours = hours;
            this.grade = grade;
        }

        public int CompareTo(Course? other)
        {
            int prefixCompare = Prefix.CompareTo(other?.Prefix);

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
