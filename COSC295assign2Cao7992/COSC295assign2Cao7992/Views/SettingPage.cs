using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace COSC295assign2Cao7992.Views
{
    public class SettingPage : ContentPage
    {
        public SettingPage(Database database)
        {
            Title = "Settings";
            Button resetButton = new Button { Text = "Reset App" };
            resetButton.Clicked += (sender, e) => { database.ResetDatabase(); };
            Content = new StackLayout { Children = { new Label { Text = "Reset all data." }, resetButton } };
        }
    }
}