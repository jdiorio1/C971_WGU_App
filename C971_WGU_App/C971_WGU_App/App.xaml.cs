using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using C971_WGU_App.Models;
using Plugin.LocalNotifications;

namespace C971_WGU_App
{
    public partial class App : Application
    {
        public static DatabaseHandler DataHandler { get; private set; }
        public static string databasePath { get; private set; }
        public App(string dbPath)
        {
            Device.SetFlags(new[] { "RadioButton_Experimental" });
            InitializeComponent();

            DataHandler = new DatabaseHandler(dbPath);
            DataHandler.CreateTables();
            databasePath = dbPath;

            MainPage = new NavigationPage(new TermsPage());
            //MainPage = new TermsPage();
            //MainPage = new FlyoutPageMain();
            getCourseStartAlerts();
            getCoursesEndAlerts();
            getAssessmentStartAlerts();
            getAssessmentsEndAlerts();
            LoadInit();

        }

        private void getAssessmentsEndAlerts()
        {
            int i = getUID();
            DateTime date = DateTime.Today;
            using (SQLiteConnection conn = new SQLiteConnection(databasePath))
            {
                //check for courses with an alert
                var assessments = (from a in conn.Table<Assessment>()
                                   where a.Alert == true
                                   select a).ToList();
                //create notification for courses within 24 hours
                foreach (var assess in assessments)
                {
                    if ((date.AddDays(1.00) == assess.EndDate) || (date==assess.EndDate))
                    {
                        CrossLocalNotifications.Current.Show("Assessment Alert", $"course {assess.Name} ends on {assess.EndDate.ToShortDateString()}", i);
                    }
                }
            }
        }
        private void getAssessmentStartAlerts()
        {
            int i = getUID();
            DateTime date = DateTime.Today;
            using (SQLiteConnection conn = new SQLiteConnection(databasePath))
            {
                //check for courses with an alert
                var assessments = (from a in conn.Table<Assessment>()
                                   where a.Alert == true
                                   select a).ToList();
                //create notification for courses within 24 hours
                foreach (var assess in assessments)
                {
                    if ((date.AddDays(1.00) == assess.StartDate) || (date==assess.StartDate))
                    {
                        CrossLocalNotifications.Current.Show("Assessment Alert", $"course {assess.Name} starts on {assess.StartDate.ToShortDateString()}", i);
                    }
                }
            }
        }


        private void getCoursesEndAlerts()
        {
            int i = getUID();
            DateTime date = DateTime.Today;
            using (SQLiteConnection conn = new SQLiteConnection(databasePath))
            {
                //check for courses with an alert
                var courses = (from c in conn.Table<Course>()
                               where c.Alert == true
                               select c).ToList();
                //create notification for courses within 24 hours
                foreach (var course in courses)
                {
                    if ((date.AddDays(1.00) == course.EndDate) || (date==course.EndDate))
                    {
                        CrossLocalNotifications.Current.Show("Course Alert", $"course {course.Name} ends on {course.StartDate.ToShortDateString()}", i);
                    }
                }
            }
        }

        private void getCourseStartAlerts()
        {
            int i = getUID();
            DateTime date = DateTime.Today;
            using (SQLiteConnection conn = new SQLiteConnection(databasePath))
            {
                //check for courses with an alert
                var courses = (from c in conn.Table<Course>()
                               where c.Alert == true
                               select c).ToList();
                //create notification for courses within 24 hours
                foreach (var course in courses)
                {
                    if ((date.AddDays(1.00) == course.StartDate) || (date == course.StartDate))
                    {
                        CrossLocalNotifications.Current.Show("Course Alert", $"course {course.Name} starts on {course.StartDate.ToShortDateString()}", i);
                    }
                }
            }
        }

        private int getUID()
        {
            byte[] gb = Guid.NewGuid().ToByteArray();
            int i = BitConverter.ToInt32(gb, 0);
            return i;
        }

        private void LoadInit()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.databasePath))
            {
                var terms = conn.Table<Term>().ToList();
                if (terms.Count == 0)
                {
                    Term term = new Term()
                    {
                        Title = "Fall 2020",
                        StartDate = Convert.ToDateTime("09/01/2021"),
                        EndDate = Convert.ToDateTime("11/30/2021")
                    };
                    conn.Insert(term);
                    Instructor instructor = new Instructor()
                    {
                        Name = "Jaime Diorio",
                        Phone = "2399800155",
                        Email = "jdiori1@wgu.edu"
                    };
                    conn.Insert(instructor);
                    Course course = new Course()
                    {
                        Name = "Apple Picking 101",
                        StartDate = Convert.ToDateTime("09/01/2021"),
                        EndDate = Convert.ToDateTime("09/30/2021"),
                        Status = "Not Started",
                        Notes = "Gala, Honeycrisp, Braeburn, Granny Smith",
                        Alert = false,
                        TermId = term.Id,
                        InstructorID = instructor.Id
                    };
                    conn.Insert(course);
                    Assessment assess1 = new Assessment()
                    {
                        Type = "Objective Assessment",
                        Name = "Test 1",
                        StartDate = Convert.ToDateTime("09/23/21"),
                        EndDate = Convert.ToDateTime("09/25/21"),
                        Alert = false,
                        CourseID = course.Id
                    };
                    conn.Insert(assess1);
                    Assessment assess2 = new Assessment()
                    {
                        Type = "Performance Assessment",
                        Name = "Test 2",
                        StartDate = Convert.ToDateTime("09/26/21"),
                        EndDate = Convert.ToDateTime("09/30/21"),
                        Alert = false,
                        CourseID = course.Id
                    };
                    conn.Insert(assess2);
                }
                else { return; }
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}
