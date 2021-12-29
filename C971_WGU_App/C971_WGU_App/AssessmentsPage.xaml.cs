using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using C971_WGU_App.Models;
using System.Collections.ObjectModel;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsPage : ContentPage
    {
        private ObservableCollection<Assessment> _assessments;
        private Course _courseSelected;
        public AssessmentsPage(Course courseSelected)
        {
            InitializeComponent();
            _courseSelected = courseSelected;
        }

        private async void AddAssessmentButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAssessmentPage(_courseSelected));
        }

        private void AssessmentList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            //Confirm that the user meant to delete the selected item
            var deleteAssess = await DisplayAlert("Alert", "Are you sure you want to delete this assessment?", "OK", "Cancel");

            //Identify the selected assessment to delete
            Assessment assessToDelete = (Assessment)((ImageButton)sender).CommandParameter;
            int assessId = assessToDelete.Id;

            if(deleteAssess==true)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.Delete<Assessment>(assessId);
                    _assessments.Remove(assessToDelete);
                }
            }
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            Assessment assessToEdit = (Assessment)((ImageButton)sender).CommandParameter;
            await Navigation.PushAsync(new EditAssessmentPage(assessToEdit));
        }

        //when page appears connect to database, check for updates to assessment table, bind to listview
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                var assessments = (from a in conn.Table<Assessment>()
                                   where a.CourseID==_courseSelected.Id
                                   select a).ToList();
                _assessments = new ObservableCollection<Assessment>(assessments);
                AssessmentList.ItemsSource = _assessments;
            }
        }

    }
}