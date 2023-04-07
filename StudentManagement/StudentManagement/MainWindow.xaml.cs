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
        //private ObservableCollection<Person> people = new ObservableCollection<Person>();
        private List<Student> students;
        public MainWindow()
        {
            InitializeComponent();
            students = new List<Student>();
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem studentListBoxItem = new ListBoxItem();
            studentListBoxItem.Content = LastNameTextBox.Text + " " + FirstNameTextBox.Text;
            StudentsListBox.Items.Add(studentListBoxItem);


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

            students.Add(s);
            RefreshStudentList();

            ClearStudentFields();
            
        }

        private void RefreshStudentList()
        {
            StudentsListBox.Items.Clear();
            students.Sort();

            foreach (Student s in students)
            {
                ListBoxItem studentListBoxItem = new ListBoxItem();
                studentListBoxItem.Content = s.LastName + " " + s.FirstName;
                StudentsListBox.Items.Add(studentListBoxItem);
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
