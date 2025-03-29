using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace COSC295assign2Cao7992
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
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
