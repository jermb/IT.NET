using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    public abstract class Student : Person
    {

        private string id;
        private int minCourseNum;
        private int maxCourseNum;
        private List<Course> courses;

        public string ID { get => id; set => id = value; }

        protected int MinCourseNum { get => minCourseNum; set => minCourseNum = value; }
        protected int MaxCourseNum { get => maxCourseNum; set => maxCourseNum = value; }
        public List<Course> Courses { get => courses; }


        public Student(string first, string last, int gender, int age, string id)
        {
            CheckEmpty(age, first, last, id);

            this.id = id;
            base.FirstName = first;
            base.LastName = last;
            base.Gender = gender;
            base.Age = age;
            courses = new List<Course>();
        }

        public void AddCourse(string name, string prefix, int number, int hours, double grade)
        {
            if (number < minCourseNum || number > maxCourseNum) throw new InvalidCourseNumberException(MaxCourseNum, MinCourseNum);

            courses.Add(new Course(name, prefix, number, hours, grade));
            courses = courses.OrderBy(c => c.Prefix).ThenBy(c=> c.Number).ToList();
        }

        public bool IsValidCourse(int num)
        {
            return num >= minCourseNum && num <= maxCourseNum;
        }

        private void CheckEmpty(int age, params string[] others)
        {
            foreach (var s in others)
            {
                if (string.IsNullOrWhiteSpace(s)) throw new ArgumentNullException();
            }
            if (age <= 0) throw new ArgumentNullException();
        }
    }
}
