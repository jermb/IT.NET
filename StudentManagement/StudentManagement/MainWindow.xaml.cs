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
        private Student? selectedStudent;
        private Course? selectedCourse;
        private Students studentList;
        private Courses courseList;
        public MainWindow()
        {
            InitializeComponent();
            //students.OrderBy(s => s.Display);
            studentList = (Students)FindResource("StudentList");
            courseList = (Courses)FindResource("CourseList");
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

            //StudentsListBox.Items.Add(s);
    
            studentList.Add(s);

            //RefreshStudentList();

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

            s.AddCourse(name, prefix, number, hours, grade);
            RefreshCourseList();
            ClearCourseFields();
            AddCourseButton.IsEnabled = false;
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
            //CoursesListBox.Items.Clear();
            //Student s = (Student)StudentsListBox.SelectedItem;
            //foreach (Course c in s.Courses)
            //{
            //    ListBoxItem courseItem = new ListBoxItem();
            //    courseItem.Content = c.Prefix + " " + c.Number;
            //    StudentsListBox.Items.Add(courseItem);
            //}
            if (selectedStudent == null) return;
            courseList.Set(selectedStudent.Courses);
            TotalGPATextBox.Text = courseList.TotalGPA.ToString();

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

        private void StudentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedStudent = (Student)StudentsListBox.SelectedItem;

            if (selectedStudent != null)
            {
                FirstNameTextBox.Text= selectedStudent.FirstName;
                LastNameTextBox.Text= selectedStudent.LastName;
                StudentIDTextBox.Text = selectedStudent.ID;
                AgeTextBox.Text = selectedStudent.Age.ToString();
                GenderComboBox.SelectedIndex = selectedStudent.Gender;
                if (selectedStudent is UndergraduateStudent) LevelComboBox.SelectedIndex = 0;
                else LevelComboBox.SelectedIndex = 1;

                courseList.Set(selectedStudent.Courses);
                TotalGPATextBox.Text = courseList.TotalGPA.ToString();
                AddStudentButton.IsEnabled = false;
            }
        }

        private void CoursesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedCourse = (Course)CoursesListBox.SelectedItem;

            if (selectedCourse != null)
            {
                CourseNameTextBox.Text = selectedCourse.Name;
                CourseNumberTextBox.Text = selectedCourse.Number.ToString();
                CoursePrefixTextBox.Text= selectedCourse.Prefix.ToString();
                CreditHoursTextBox.Text = selectedCourse.CreditHours.ToString();
                GPATextBox.Text = selectedCourse.Grade.ToString();
                AddCourseButton.IsEnabled = false;
            }

        }

        private void StudentField_Changed(object sender, RoutedEventArgs e)
        {
            if (AddStudentButton == null) return;
            AddStudentButton.IsEnabled = true;
        }

        private void CourseField_Changed(object sender, RoutedEventArgs e)
        {
            if (AddCourseButton == null) return;
            AddCourseButton.IsEnabled = true;
        }
    }
}
