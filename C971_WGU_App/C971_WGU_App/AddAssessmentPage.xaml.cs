using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using C971_WGU_App.Models;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddAssessmentPage : ContentPage
    {
        private Course _courseSelected;
        private bool _alerts;
        public AddAssessmentPage(Course courseSelected)
        {
            InitializeComponent();
            _courseSelected = courseSelected;
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                var assessmentsList = (from a in conn.Table<Assessment>()
                                       where a.CourseID == _courseSelected.Id select a).ToList();
                
                switch (assessmentsList.Count)
                {
                    case 0:
                        AssessTypePicker.Items.Add("Objective Assessment");
                        AssessTypePicker.Items.Add("Performance Assessment");
                        break;
                    case 1:
                        {
                            if (assessmentsList[0].Type.Equals("Objective Assessment"))
                            {
                                AssessTypePicker.Items.Add("Performance Assessment");
                            }
                            else
                            {
                                AssessTypePicker.Items.Add("Objective Assessment");
                            }
                        }
                        break;
                    case 2:
                        {
                            SaveButton.IsEnabled = false;
                        }
                        break;
                }
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            if(AssessmentName != null)
            {
                if(AssessTypePicker.SelectedItem != null)
                {
                    Models.Assessment assessToAdd = new Models.Assessment()
                    {
                        Name = AssessmentName.Text,
                        Type=AssessTypePicker.SelectedItem.ToString(),
                        StartDate=startDatePicker.Date,
                        EndDate=endDatePicker.Date,
                        Alert=AlertStatus(),
                        CourseID=_courseSelected.Id
                    };
                    using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                    {
                        conn.Insert(assessToAdd);
                    }
                    //Go back to terms page after save completes
                    Navigation.PopAsync();
                }
                else
                {
                    Application.Current.MainPage.DisplayAlert("Alert", "Please choose an assessment type", "OK");
                }
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Please enter a name", "OK");
            }
        }

        private void NoButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
            {
                _alerts = false;

            }
        }

        private void YesButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == true)
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