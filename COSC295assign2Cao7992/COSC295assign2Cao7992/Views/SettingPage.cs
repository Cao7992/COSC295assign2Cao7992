using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace COSC295assign2Cao7992.Views
{
    /*
     * Setting page contains a button to reset data to default state, and discription to justify for this button
     * the page is displayed as Stack veritically
     */
    public class SettingPage : ContentPage
    {
        public SettingPage(Database database)
        {
            Title = "Settings";
            Label lblDescription = new Label
            {
                Padding = 10,
                Text = "Clicking on the button will reset the database to default data. In other words, there is will be no records for Opponents Page, no records for Matches Page. " +
                "3 predefined Games without any related Match",
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            Button resetButton = new Button { Text = "Reset App" };
            resetButton.Clicked += (sender, e) => { database.ResetDatabase(); };
            Content = new StackLayout 
            {
                Padding = 5,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children = { lblDescription, resetButton } 

            };
        }
    }
}