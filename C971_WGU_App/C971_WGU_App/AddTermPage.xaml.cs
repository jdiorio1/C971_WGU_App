using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTermPage : ContentPage
    {
        public AddTermPage()
        {
            InitializeComponent();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            //Make sure the title is populated and if it is add the item to the terms table
            if (TermTitle.Text != null)
            {
                Models.Term termToAdd = new Models.Term()
                {
                    Title = TermTitle.Text,
                    StartDate = startDatePicker.Date,
                    EndDate = endDatePicker.Date,
                };

                using(SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.Insert(termToAdd);
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