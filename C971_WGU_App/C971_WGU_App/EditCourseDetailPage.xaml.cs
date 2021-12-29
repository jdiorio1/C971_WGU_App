using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971_WGU_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Text.RegularExpressions;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditCourseDetailPage : ContentPage
    {
        private Course _courseToEdit;
        private Instructor _instructorToEdit;
        private bool _instructorNameValid;
        private bool _instructorPhoneValid;
        private bool _instructorEmailValid;
        private string _selectedCourseStatus;
        private bool _alerts;
        public EditCourseDetailPage(Course courseToEdit, Instructor instructorToEdit)
        {
            InitializeComponent();
            _courseToEdit = courseToEdit;
            _instructorToEdit = instructorToEdit;

            //set binding context for course properties to course object
            CourseName.BindingContext = _courseToEdit;
            startDatePicker.BindingContext = _courseToEdit;
            endDatePicker.BindingContext = _courseToEdit;
            courseStatus.BindingContext = _courseToEdit;
            Notes.BindingContext = _courseToEdit;
            YesButton.BindingContext = _courseToEdit;
            NoButton.BindingContext = _courseToEdit;
            //set binding context for instructor properties to instructor object
            InstructorName.BindingContext = _instructorToEdit;
            InstructorPhone.BindingContext = _instructorToEdit;
            InstructorEmail.BindingContext = _instructorToEdit;
            //check which alert option is selected and populate accordingly
            if(_courseToEdit.Alert==true)
            {
                YesButton.IsChecked = true;
            }
            else
            {
                NoButton.IsChecked = true;
            }

            foreach(string item in courseStatus.Items)
            {
                if (item.Equals(_courseToEdit.Status))
                {
                    courseStatus.SelectedItem = item;
                }
            }

            //courseStatus.SelectedItem = courseStatus.Items.Where(pickerItem => pickerItem.Equals(_courseToEdit.Status)).FirstOrDefault();

        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            //Make sure the Course name is populated and if it is then check the instructor information
            if (CourseName.Text != null)
            {
                //create string variables that contain the patterns to check instructor info against
                //Instructor Name Pattern
                string namePattern = @"^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)";
                //Instructor Email address pattern
                string emailPattern = @"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$";
                //Instructor Phone Pattern
                string phonePattern = @"^\d{10}$";

                //use Regex.IsMatch to check entered value against pattern to validate and store result in bool
                _instructorNameValid = Regex.IsMatch(InstructorName.Text, namePattern);
                _instructorPhoneValid = Regex.IsMatch(InstructorPhone.Text, phonePattern);
                _instructorEmailValid = Regex.IsMatch(InstructorEmail.Text, emailPattern);

                //check result for each
                if (!_instructorNameValid || InstructorName.Text is null)
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "Please enter valid instructor name", "OK");
                }
                else if (!_instructorPhoneValid || InstructorPhone.Text is null)
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "Please enter a valid phone number", "OK");
                }
                else if (!_instructorEmailValid || InstructorEmail is null)
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "Please enter a valid email address", "OK");
                }
                else
                {
                    //Once values have been checked and validated add data to appropriate tables

                    Models.Instructor instructorToUpdate = new Models.Instructor()
                    {
                        Id = _instructorToEdit.Id,
                        Name = InstructorName.Text,
                        Phone = InstructorPhone.Text,
                        Email = InstructorEmail.Text
                    };

                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.Update(instructorToUpdate);
                    }

                    //if course status is populated retrieve the selected item and if not value is null
                    if (courseStatus.SelectedItem != null)
                    {
                        _selectedCourseStatus = courseStatus.SelectedItem.ToString();
                    }
                    else
                    {
                        _selectedCourseStatus = null;
                    }

                    Models.Course courseToUpdate = new Models.Course()
                    {
                        Id = _courseToEdit.Id,
                        Name = CourseName.Text,
                        StartDate = startDatePicker.Date,
                        EndDate = endDatePicker.Date,
                        Status = _selectedCourseStatus,
                        Notes = Notes.Text,
                        Alert = AlertStatus(),
                        InstructorID = instructorToUpdate.Id,
                        TermId = _courseToEdit.TermId
                    };


                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.Update(courseToUpdate);
                    }

                    //Go to terms page once course has been added
                    Navigation.PopAsync();
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Please enter a course name", "OK");
            }
        }

        private void YesButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                _alerts = true;
            }
            else
            {
                _alerts = false;
            }
        }

        private void NoButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                _alerts = false;

            }
            else
            {
                _alerts = true;
            }
        }
        private bool AlertStatus()
        {
            if (YesButton.IsChecked == true)
            {
                return true;
            }
            if (NoButton.IsChecked == true)
            {
                return false;
            }
            else { return false; }
        }
    }
}