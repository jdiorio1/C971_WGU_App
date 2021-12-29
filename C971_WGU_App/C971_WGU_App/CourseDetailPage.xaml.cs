using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C971_WGU_App.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using SQLite;

namespace C971_WGU_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseDetailPage : ContentPage
    {
        private Course _courseToView;
        private Instructor _courseInstructor;
        public CourseDetailPage(Course courseToView, Instructor courseInstructor)
        {
            InitializeComponent();
            _courseToView = courseToView;
            _courseInstructor = courseInstructor;
            BindContexts(courseToView, courseInstructor);
        }

        private async void Handle_Tapped(object sender, EventArgs e)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text=Notes.Text,
                Title= "Share Notes"
            });

        }

        private async void EditCourseDetails_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditCourseDetailPage(_courseToView, _courseInstructor));
        }

        private async void GoToAssessments_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssessmentsPage(_courseToView));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                Course updatedCourseToView = (from c in conn.Table<Course>()
                                              where c.Id == _courseToView.Id
                                              select c).FirstOrDefault();

                Instructor updatedInstructorToView = (from i in conn.Table<Instructor>()
                                                      where i.Id == _courseToView.InstructorID
                                                      select i).FirstOrDefault();
                BindContexts(updatedCourseToView, updatedInstructorToView);
            }
        }

        private void BindContexts(Course course, Instructor instructor)
        {

            //set binding context for course properties to course object
            CourseName.BindingContext = course;
            startDate.BindingContext = course;
            endDate.BindingContext = course;
            courseStatus.BindingContext = course;
            Notes.BindingContext = course;
            //set binding context for instructor properties to instructor object
            InstructorName.BindingContext = instructor;
            InstructorPhone.BindingContext = instructor;
            InstructorEmail.BindingContext = instructor;
        }
    }
}