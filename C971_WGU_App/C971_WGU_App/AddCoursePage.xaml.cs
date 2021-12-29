using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Text.RegularExpressions;
using C971_WGU_App.Models;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCoursePage : ContentPage
    {
        private bool _instructorNameValid;
        private bool _instructorPhoneValid;
        private bool _instructorEmailValid;
        private string _selectedCourseStatus;
        private bool _alerts;
        public AddCoursePage()
        {
            InitializeComponent();
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                var terms = conn.Table<Term>().ToList();
                TermPicker.ItemsSource = terms;
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            //make sure a term is selected and then store the termId in a variable
            if (TermPicker.SelectedItem != null)
            {
                Term selectedTerm = (Term)TermPicker.SelectedItem;
                int termId = selectedTerm.Id;

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

                        Models.Instructor instructorToAdd = new Models.Instructor()
                        {
                            Name = InstructorName.Text,
                            Phone = InstructorPhone.Text,
                            Email = InstructorEmail.Text
                        };

                        using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                        {
                            conn.Insert(instructorToAdd);
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

                        //check to see if user wants alerts
                        

                        Models.Course courseToAdd = new Models.Course()
                        {
                            Name = CourseName.Text,
                            StartDate = startDatePicker.Date,
                            EndDate = endDatePicker.Date,
                            Status = _selectedCourseStatus,
                            Notes = Notes.Text,
                            Alert = AlertStatus(),
                            InstructorID = instructorToAdd.Id,
                            TermId = termId
                        };


                        using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                        {
                            conn.Insert(courseToAdd);
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

            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Please select a term", "OK");
            }
        }

        private void YesButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value==true)
            {
                _alerts = true;
            }
        }

        private void NoButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value==true)
            {
                _alerts = false;

            }
        }

        private bool AlertStatus()
        {
            if (YesButton.IsChecked==true)
            {
                return true;
            }
            if (NoButton.IsChecked==true)
            {
                return false;
            }
            else { return false; }
        }
    }
}