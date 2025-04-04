using COSC295assign2Cao7992.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COSC295assign2Cao7992
{
    public partial class App : Application
    {
        // Create database variable to connect to our database and is used accross page/class in the app
        static Database database;
        // helper method to connect to the database and 
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database(DependencyService.Get<IFileHelper>().GetLocalFilePath("cosc295assign2.db3"));
                }
                return database;
            }
        }
        public App()
        {
            InitializeComponent();
            database = Database;

            // Create a Toolbar with 3 buttons leading to 3 different Pages
            MainPage = new NavigationPage(new OpponentsPage(database));
            ToolbarItem tb1 = new ToolbarItem { Text = "Games" };
            tb1.Clicked += (s, e) => { MainPage.Navigation.PushAsync(new GamesPage(database)); };
            ToolbarItem tb2 = new ToolbarItem { Text = "Settings" };
            tb2.Clicked += (s, e) => { MainPage.Navigation.PushAsync(new SettingPage(database)); };
            ToolbarItem tb3 = new ToolbarItem { Text = "Home" };
            tb3.Clicked += (s, e) => { MainPage.Navigation.PopToRootAsync(); };
            
            // add the Toolbar items above into Mainpage's Toolbar
            MainPage.ToolbarItems.Add(tb1);
            MainPage.ToolbarItems.Add(tb2);
            MainPage.ToolbarItems.Add(tb3);

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
    /*
     * Interface to get full path to storage location, depending on different platform
     */
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
