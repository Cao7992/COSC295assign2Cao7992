using COSC295assign2Cao7992.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace COSC295assign2Cao7992.Views
{
    public class GamesPage : ContentPage
    {
        private const int ListViewHeight = 500;
        private static Database gDatabase;
        private static ObservableCollection<Game> lstGames { get; set; }
        public GamesPage(Database database)
        {
            gDatabase = database;
            Title = "Games";

            // Fetch Games from database
            lstGames = new ObservableCollection<Game>(gDatabase.GetGames());

            ListView listView = new ListView
            {
                ItemsSource = lstGames,
                RowHeight = GameCell.GameCellHeight,
                ItemTemplate = new DataTemplate(typeof(GameCell)),
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = ListViewHeight,
            };
            Content = new StackLayout { Padding = 20, Children = { listView } };
        }
        /* 
         * method to handle action when the page is reloaded/ returned
         * also, it reload/update the database to itemsoucre of list view,
         * when there is the change on the number of matches of each game
         */
        protected override void OnAppearing()
        {
            base.OnAppearing();
            var gamesFromDb = gDatabase.GetGames();
            lstGames.Clear(); // Clear old data
            foreach (Game game in gamesFromDb)
            {
                lstGames.Add(game); // Add updated data
            }
        }

        /*
     * Helper Class: 
     * Purpose: to layout each cell in the list View
     * Conponent include: height property, Gamename, Desciption, Rating, MatchesCount property of Game object
     */
        internal class GameCell : ViewCell
        {
            public const int GameCellHeight = 120;
            public GameCell()
            {
                // genetate components of the viewCell
                Label lblGameName = new Label { FontAttributes = FontAttributes.Bold, FontSize = 20 };
                Label lblDesc = new Label { FontAttributes = FontAttributes.None, FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand };
                Label lblRating = new Label { FontAttributes = FontAttributes.None, FontSize = 20, HorizontalOptions = LayoutOptions.End };
                Label lblMatchesCount = new Label { FontAttributes = FontAttributes.Bold, TextColor = Color.Purple, FontSize = 20,  };
                
                // Bind the components with the BindingContext 
                lblGameName.SetBinding(Label.TextProperty, "GameName");
                lblDesc.SetBinding(Label.TextProperty, "Description");
                lblRating.SetBinding(Label.TextProperty, "Rating");
                lblMatchesCount.SetBinding(Label.TextProperty, "ID", converter: new GameIdConvert());

                // bound two components Description and Rating together to display them on 1 line
                StackLayout stack1 = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { lblDesc, lblRating }
                };

                // bound component lblMatchesCount with a label bofore it to display them on 1 line
                StackLayout stack2 = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children =
                    {
                        new Label { Text = "#Matches: ", FontSize = 20},
                        lblMatchesCount,
                    }
                };

                View = new StackLayout
                {
                    Spacing = 10,
                    Padding = 10,
                    Children = { lblGameName, stack1, stack2 },
                    Orientation = StackOrientation.Vertical
                };
            }

            public class GameIdConvert : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    return GamesPage.gDatabase.GetMatchesByGame(int.Parse(value.ToString())).Count().ToString();
                }
                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}