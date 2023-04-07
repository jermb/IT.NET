using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public ObservableCollection<Person> people = new ObservableCollection<Person>();
        ////public ObservableCollection<Student> students = new ObservableCollection<Student>();
        //public List<Student> students = new List<Student>();
        //private Student? selectedStudent;
        public MainWindow()
        {
            InitializeComponent();
            //students.OrderBy(s => s.Display);
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            //  Need to handle empty boxes
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string studentID = StudentIDTextBox.Text;
            int gender = GenderComboBox.SelectedIndex;
            int age = int.Parse(AgeTextBox.Text);
            int level = LevelComboBox.SelectedIndex;

            Student s;

            if (level == 0) s = new UndergraduateStudent(firstName, lastName, gender, age, studentID);
            else s = new GraduateStudent(firstName, lastName, gender, age, studentID);

            StudentsListBox.Items.Add(s);
            RefreshStudentList();

            ClearStudentFields();
            
        }

        private void AddCourseButton_Click(object sender, RoutedEventArgs e)
        {
            string name = CourseNameTextBox.Text;
            string prefix = CoursePrefixTextBox.Text;
            int number = int.Parse(CourseNumberTextBox.Text);
            int hours = int.Parse(CreditHoursTextBox.Text);
            double grade = double.Parse(GPATextBox.Text);

            Student s = (Student)StudentsListBox.SelectedItem;

            if (s.Add(name, prefix, number, hours, grade))
            {
                RefreshCourseList();
            }
        }

        private void RefreshStudentList()
        {
            StudentsListBox.Items.Clear();

            foreach (Student s in StudentsListBox.Items)
            {
                ListBoxItem studentListBoxItem = new ListBoxItem();
                studentListBoxItem.Content = s.LastName + " " + s.FirstName;
                StudentsListBox.Items.Add(studentListBoxItem);
            }
        }

        private void RefreshCourseList()
        {
            CoursessListBox.Items.Clear();
            Student s = (Student)StudentsListBox.SelectedItem;
            foreach (Course c in s.Courses)
            {
                ListBoxItem courseItem = new ListBoxItem();
                courseItem.Content = c.Prefix + " " + c.Number;
                StudentsListBox.Items.Add(courseItem);
            }
        }

        private void ClearStudentFields()
        {
            FirstNameTextBox.Text = "";
            LastNameTextBox.Text = "";
            StudentIDTextBox.Text = "";
            AgeTextBox.Text = "";
        }

        private void ClearCourseFields()
        {
            CourseNameTextBox.Text = "";
            CourseNumberTextBox.Text = "";
            CoursePrefixTextBox.Text = "";
            CreditHoursTextBox.Text = "";
            GPATextBox.Text = "";
        }


    }
}
