using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971_WGU_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditAssessmentPage : ContentPage
    {
        private Assessment _assessToEdit;
        private bool _alerts;
        public EditAssessmentPage(Assessment assessToEdit)
        {
            InitializeComponent();
            _assessToEdit = assessToEdit;
            BindingContext = _assessToEdit;

            //check which alert option is selected and populate accordingly
            if (assessToEdit.Alert == true)
            {
                YesButton.IsChecked = true;
            }
            else
            {
                NoButton.IsChecked = true;
            }
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            //Make sure title is populated and if it is update the object in the assessment table
            if(!string.IsNullOrEmpty(AssessmentName.Text))
            {
                Models.Assessment assessToUpdate = new Models.Assessment()
                {
                    Id = this._assessToEdit.Id,
                    Name = AssessmentName.Text,
                    Type=_assessToEdit.Type,
                    StartDate = startDatePicker.Date,
                    EndDate = endDatePicker.Date,
                    Alert=AlertStatus(),
                    CourseID = _assessToEdit.CourseID
                };
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.Update(assessToUpdate);
                }
                //go back to assessments once save completes
                Navigation.PopAsync();
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Please enter a name", "OK");
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