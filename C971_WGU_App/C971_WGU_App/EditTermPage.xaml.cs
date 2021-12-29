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
    public partial class EditTermPage : ContentPage
    {
        private Term _termToEdit;

        public EditTermPage(Term termToEdit)
        {
            InitializeComponent();
            _termToEdit = termToEdit;
            BindingContext = _termToEdit;
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            //Make sure the title is populated and if it is update the item in the terms table
            if (!string.IsNullOrEmpty(TermTitle.Text))
            {
                Models.Term termToUpdate = new Models.Term()
                {
                    Id = this._termToEdit.Id,
                    Title = TermTitle.Text,
                    StartDate = startDatePicker.Date,
                    EndDate = endDatePicker.Date,
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.Update(termToUpdate);
                }
                //Go back to terms page after save completes
                Navigation.PopAsync();
            }

            //If title isn't populated display an alert to the user and don't save
            else
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Please enter a title", "OK");
            }
        }
    }
}