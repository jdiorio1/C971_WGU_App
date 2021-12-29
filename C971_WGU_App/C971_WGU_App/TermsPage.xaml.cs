using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using C971_WGU_App.Models;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using Plugin.LocalNotifications;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsPage : ContentPage
    {
        ObservableCollection<Term> _terms;
        public TermsPage()
        {
            InitializeComponent();
        }

        private async void AddTermButton_Clicked(object sender, EventArgs e)
        {
            await this.Navigation.PushAsync(new AddTermPage());
        }

        //when page appears connect to database, check for updates to terms table, bind to listview
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                var terms = conn.Table<Term>().ToList();
                _terms = new ObservableCollection<Term>(terms);

                TermsList.ItemsSource = _terms;
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {

            //Confirm that the user meant to delete the selected item
            var deleteTerm = await DisplayAlert("Alert", "Are you sure you want to delete this term? All associated courses will also be deleted.", "OK", "Cancel");

            //Identify the selected term to delete
            Term recordToDelete = (Term)((ImageButton)sender).CommandParameter;
            int termId = recordToDelete.Id;

                if (deleteTerm == true)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    //check to see if there are any courses associated with the selected term
                    var courses = (from c in conn.Table<Course>()
                                   where c.TermId == recordToDelete.Id
                                   select c.Id).ToList();

                    //delete courses first
                    foreach (var courseId in courses)
                    {
                        conn.Delete<Course>(courseId);
                    }

                    //then delete the term
                    conn.Delete<Term>(termId);
                    _terms.Remove(recordToDelete);
                }

            }

        }

        private async void TermsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Term termSelected = (Term)e.SelectedItem;
            await this.Navigation.PushAsync(new CoursesPage(termSelected));
        }

        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            Term termToEdit = (Term)((ImageButton)sender).CommandParameter;
            await this.Navigation.PushAsync(new EditTermPage(termToEdit));
        }
    }
}