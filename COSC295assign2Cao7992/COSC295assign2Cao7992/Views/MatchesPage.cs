using COSC295assign2Cao7992.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace COSC295assign2Cao7992.Views
{
    public class MatchesPage2 : ContentPage
    {
        private ListView _listView;
        private Database _database;
        private int _opponentId;

        public MatchesPage2(Database database, int opponentId)
        {
            _database = database;
            _opponentId = opponentId;
            Title = "Matches";

            _listView = new ListView { ItemsSource = _database.GetMatchesByOpponent(opponentId) };
            Content = new StackLayout { Children = { _listView } };
        }
    }

    public class MatchesPage : ContentPage
    {
        private readonly Database mDatabase;
        private readonly int mOpponentId;
        private readonly ListView mListView;

        private readonly Picker tbGamePicker;
        private readonly DatePicker tbDatePicker;
        private readonly Label tbCommentCell;
        private readonly Switch tbWinSwitch;
        private readonly Button btnAddMatch;

        private ObservableCollection<Match> lstMatches { get; set; }
        private ObservableCollection<Game> lstGames { get; set; }
        //private string SelectedGame { get; set; }

        public MatchesPage(Database database, int opponentId)
        {
            mDatabase = database;
            mOpponentId = opponentId;

            Title = "Matches";

            // Fetch Matches and Games from database
            lstMatches = new ObservableCollection<Match>(mDatabase.GetMatchesByOpponent(opponentId));
            lstGames = new ObservableCollection<Game>(mDatabase.GetGames());

            // ListView for displaying matches
            mListView = new ListView
            {
                ItemsSource = lstMatches,
                SelectionMode = ListViewSelectionMode.None,
                ItemTemplate = new DataTemplate(() =>
                {
                    var dateLabel = new Label { WidthRequest = 100 };
                    dateLabel.SetBinding(Label.TextProperty, "Date", stringFormat: "{0:yyyy-MM-dd}");

                    var gameLabel = new Label { WidthRequest = 100 };
                    gameLabel.SetBinding(Label.TextProperty, "GameName");

                    var winLabel = new Label { WidthRequest = 50 };
                    winLabel.SetBinding(Label.TextProperty, "Win");

                    var stack = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children = { dateLabel, gameLabel, winLabel }
                    };

                    var viewCell = new ViewCell { View = stack };
                    //viewCell.Tapped += (s, e) => DeleteMatch((Match)((ViewCell)s).BindingContext);
                    return viewCell;
                })
            };

            // Picker for selecting a game
            tbGamePicker = new Picker
            {
                Title = "Select Game",
                ItemsSource = lstGames,
                ItemDisplayBinding = new Binding("GameName"),
                SelectedItem = Preferences.Get("LastGameUsed", "Chess")
            };
            //tbGamePicker.SelectedIndexChanged += (s, e) => SelectedGame = _gamePicker.SelectedItem.ToString();

            // Date Picker
            tbDatePicker = new DatePicker { Date = DateTime.Today };

            // Comment 
            tbCommentCell = new  Label { Text = "Comment: " };

            // Win Switch
            tbWinSwitch = new Switch();

            // Add Button at the end of table
            btnAddMatch = new Button { Text = "Add" };
            btnAddMatch.Clicked += SaveMatch;

            // Layout
            Content = new StackLayout
            {
                Padding = 10,
                Children =
                {
                    mListView,
                    new Label { Text = "Select Game:" }, tbGamePicker,
                    new Label { Text = "Date:" }, tbDatePicker,
                    new Label { Text = "Win:" }, tbWinSwitch,
                    btnAddMatch
                }
            };
        }

        private void SaveMatch(object sender, EventArgs e)
        {
            if (tbGamePicker.SelectedIndex < 0) return;

            var match = new Match
            {
                OpponentID = mOpponentId,
                GameID = tbGamePicker.SelectedIndex,
                Date = tbDatePicker.Date,
                Win = tbWinSwitch.IsToggled
            };

            mDatabase.SaveMatch(match);
            lstMatches.Add(match);
            Preferences.Set("LastGameUsed", ((Game)tbGamePicker.SelectedItem).GameName);
        }

        private void DeleteMatch(Match match)
        {
            if (match == null) return;
            mDatabase.DeleteMatch(match);
            lstMatches.Remove(match);
        }
    }
}
