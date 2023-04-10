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
        public Courses() { }
        public double TotalGPA
        {
            get
            {
                double totalGPA = 0;
                double totalHours = 0;

                foreach (Course c in this)
                {
                    totalGPA += c.Grade * c.CreditHours;
                    totalHours += c.CreditHours;
                }

                return Math.Round(totalGPA/totalHours, 2);
            }
        }

        public void Set(List<Course> courses)
        {
            //  Clear out old list then add items of new list (already sorted when item is added in Student.cs)
            Clear();
            foreach (Course c in courses) Add(c);
        }
    }
}
