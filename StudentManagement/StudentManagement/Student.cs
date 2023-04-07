using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    public abstract class Student : Person, INotifyPropertyChanged, IEditableObject
    {

        private string id;
        private int minCourseNum;
        private int maxCourseNum;
        private List<Course> courses;

        protected int MinCourseNum { get => minCourseNum; set => minCourseNum = value; }
        protected int MaxCourseNum { get => maxCourseNum; set => maxCourseNum = value; }
        public List<Course> Courses { get => courses; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public Student(string first, string last, int gender, int age, string id)
        {
            this.id = id;
            base.FirstName = first;
            base.LastName = last;
            base.Gender = gender;
            base.Age = age;
            courses = new List<Course>();
        }

        public bool Add(string name, string prefix, int number, int hours, double grade)
        {
            if (number < minCourseNum || number > maxCourseNum) { /* Throw Error */ return false; }

            courses.Add(new Course(name, prefix, number, hours, grade));
            courses.Sort();
            return true;
        }

        public void BeginEdit()
        {
            throw new NotImplementedException();
        }

        public void CancelEdit()
        {
            throw new NotImplementedException();
        }

        public void EndEdit()
        {
            throw new NotImplementedException();
        }

        public bool IsValidCourse(int num)
        {
            return num >= minCourseNum && num <= maxCourseNum;
        }
    }
}
