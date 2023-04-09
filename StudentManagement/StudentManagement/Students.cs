using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    class Students : ObservableCollection<Student>
    {
        public Students() {
            Student s = new GraduateStudent("epid", "ald", 0, 22, "asldkf");
            s.AddCourse("Course", "Crs", 6660, 3, 3.2);

            Add(s);
            
        }

        public new void Add(Student student)
        {
            //  Adds new student to collection
            base.Add(student);
            //  Creates a temp list of collections items sorted by Last name then first name
            List<Student> studentList = this.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
            //  Clears out the collection
            this.Clear();
            //  Add each student back into the collection now properly sorted
            foreach (Student s in studentList)
            {
                base.Add(s);
            }
        }
    }
}
