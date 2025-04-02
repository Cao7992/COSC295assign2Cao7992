using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace COSC295assign2Cao7992.Views
{
    public class GamesPage : ContentPage
    {
        public GamesPage(Database database)
        {
            Title = "Games";
            var listView = new ListView { ItemsSource = database.GetGames() };
            Content = new StackLayout { Children = { listView } };
        }
    }
}