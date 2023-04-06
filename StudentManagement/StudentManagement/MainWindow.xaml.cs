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
        public MainWindow()
        {
            InitializeComponent();
        }
        private void AddStudentButton_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem studentListBoxItem = new ListBoxItem();
            studentListBoxItem.Content = LastNameTextBox.Text + " " + FirstNameTextBox.Text;
            StudentsListBox.Items.Add(studentListBoxItem);
            ClearStudentFields();
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
