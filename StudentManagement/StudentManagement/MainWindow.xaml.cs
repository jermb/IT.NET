using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private Student? selectedStudent;
        private Course? selectedCourse;
        private Students studentList;
        private Courses courseList;
        public MainWindow()
        {
            InitializeComponent();
            studentList = (Students)FindResource("StudentList");
            courseList = (Courses)FindResource("CourseList");
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            //  Grab the info from the input fields
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string studentID = StudentIDTextBox.Text;
            int gender = GenderComboBox.SelectedIndex;
            int level = LevelComboBox.SelectedIndex;
            //int age = int.Parse(AgeTextBox.Text);
            int.TryParse(AgeTextBox.Text, out int age);

            try
            {
                //  Create the appropriate student type based on the selected level
                Student student;
                if (level == 0) student = new UndergraduateStudent(firstName, lastName, gender, age, studentID);
                else student = new GraduateStudent(firstName, lastName, gender, age, studentID);

                studentList.Add(student);
                ClearStudentFields();
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("All fields must be filled.", "Alert");
                if (age == 0) AgeTextBox.Text = "";
            }
     
        }

        private void AddCourseButton_Click(object sender, RoutedEventArgs e)
        {
            string name = CourseNameTextBox.Text;
            string prefix = CoursePrefixTextBox.Text;
            int number = int.Parse(CourseNumberTextBox.Text);
            int hours = int.Parse(CreditHoursTextBox.Text);
            double grade = double.Parse(GPATextBox.Text);

            try
            {
                selectedStudent?.AddCourse(name, prefix, number, hours, grade);
                RefreshCourseList();
                ClearCourseFields();
                AddCourseButton.IsEnabled = false;
            }
            catch (InvalidCourseNumberException ex)
            {
                BadInputBox(ex.Message, CourseNumberTextBox);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                BadInputBox(ex.Message, GPATextBox);
            }
        }

        private void RefreshCourseList()
        {
            if (selectedStudent == null) return;
            //  Reset the Course List to display the current student's courses
            courseList.Set(selectedStudent.Courses);
            //  Calculate and update the TotalGPA
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
            courseList.Clear();
            ClearCourseFields();
            AddCourseButton.IsEnabled = false;
            selectedStudent = (Student)StudentsListBox.SelectedItem;

            if (selectedStudent != null)
            {
                //  Fill out student fields
                FirstNameTextBox.Text= selectedStudent.FirstName;
                LastNameTextBox.Text= selectedStudent.LastName;
                StudentIDTextBox.Text = selectedStudent.ID;
                AgeTextBox.Text = selectedStudent.Age.ToString();
                GenderComboBox.SelectedIndex = selectedStudent.Gender;
                if (selectedStudent is UndergraduateStudent) LevelComboBox.SelectedIndex = 0;
                else LevelComboBox.SelectedIndex = 1;

                //  Update course list and GPA
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
                //  Fill out course fields
                CourseNameTextBox.Text = selectedCourse.Name;
                CourseNumberTextBox.Text = selectedCourse.Number.ToString();
                CoursePrefixTextBox.Text= selectedCourse.Prefix.ToString();
                CreditHoursTextBox.Text = selectedCourse.CreditHours.ToString();
                GPATextBox.Text = selectedCourse.Grade.ToString();
                //  Disable button
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

        private void BadInputBox(string message, TextBox tb)
        {
            MessageBox.Show(message, "Alert");
            tb.Text = "";
        }

        //  Prevent the user from inputting non-numbers in select boxes
        private void CheckIntegerInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void CheckDoubleInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text);
        }
    }
}
