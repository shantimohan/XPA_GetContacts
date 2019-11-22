using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XPA_GetContacts
{
    public partial class App : Application
    {
        public const string AppNameAcronym = "Get Contacts";
        public static string appVersion = "1.0";

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        internal static void SetAppTitleAndToday(Label lblAppTitle, Label lblTodayIs)
        {
            lblAppTitle.Text = $"{App.AppNameAcronym} {App.appVersion}";
            DateTime today = DateTime.Today;
            string dtSuffix = "";
            switch (today.Day)
            {
                case 1:
                    dtSuffix = "st";
                    break;
                case 2:
                    dtSuffix = "nd";
                    break;
                case 3:
                    dtSuffix = "rd";
                    break;
                default:
                    dtSuffix = "th";
                    break;
            }
            lblTodayIs.Text = $"{today.ToString("MMM")} {today.Day}{dtSuffix}, {today.Year}";
        }
    }
}
