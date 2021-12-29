using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using C971_WGU_App.Models;
using System.Collections.ObjectModel;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CoursesPage : ContentPage
    {
        private ObservableCollection<Course> _courses;
        private Term _termSelected;
        public CoursesPage(Term termSelected)
        {
            InitializeComponent();
            this._termSelected = termSelected;
        }

        //when page appears connect to database, check for updates to courses table, bind to listview
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                var instructors = conn.Table<Instructor>().ToList();
                int total = instructors.Count;
                var courses = (from c in conn.Table<Course>()
                               where c.TermId == _termSelected.Id
                               select c).ToList();
                 _courses = new ObservableCollection<Course>(courses);
                CourseList.ItemsSource = _courses;

            }
        }

        private async void CourseList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Course courseToView = (Course)e.SelectedItem;
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                Instructor courseInstructor = (from i in conn.Table<Instructor>()
                                               where i.Id == courseToView.InstructorID
                                               select i).FirstOrDefault();
                await this.Navigation.PushAsync(new CourseDetailPage(courseToView, courseInstructor));
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            //confirm that the user wants to delete the course
            var deleteCourse = await DisplayAlert("Alert", "Are you sure you want to delete this course?", "OK", "Cancel");

            //identify the selected course that the delete button belongs to
            Course courseToDelete = (Course)((ImageButton)sender).CommandParameter;
            int courseId = courseToDelete.Id;

            if (deleteCourse==true)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
                {
                    conn.Delete<Course>(courseId);
                    _courses.Remove(courseToDelete);
                }
            }
        }

        private async void AddCourseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCoursePage());
        }
    }
}