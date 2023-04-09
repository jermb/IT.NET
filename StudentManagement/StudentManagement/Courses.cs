using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    public class Courses : ObservableCollection<Course>
    {
        private double totalGPA;

        public double TotalGPA { get => totalGPA; }
        public Courses()
        {
            Add(new Course("course", "it", 2220, 2, 2.1));
        }

        public void Set(List<Course> courses)
        {
            Clear();
            foreach (Course c in courses) Add(c);
        }

        private void CalculateGPA()
        {
            totalGPA = 0;

            foreach (Course c in this)
            {
                totalGPA += c.Grade;
            }
        }
    }
}
