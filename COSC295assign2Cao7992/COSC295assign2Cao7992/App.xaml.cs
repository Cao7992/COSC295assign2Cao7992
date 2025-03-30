using COSC295assign2Cao7992.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COSC295assign2Cao7992
{
    public partial class App : Application
    {
        static Database database;
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
            MainPage = new NavigationPage(new OpponentsPage(database));
            ToolbarItem tb1 = new ToolbarItem { Text = "Games" };
            tb1.Clicked += (s, e) => { MainPage.Navigation.PushAsync(new GamesPage(database)); };
            ToolbarItem tb2 = new ToolbarItem { Text = "Settings" };
            tb2.Clicked += (s, e) => { MainPage.Navigation.PushAsync(new SettingsPage(database)); };
            ToolbarItem tb3 = new ToolbarItem { Text = "Home" };
            tb3.Clicked += (s, e) => { MainPage.Navigation.PopToRootAsync(); };
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
