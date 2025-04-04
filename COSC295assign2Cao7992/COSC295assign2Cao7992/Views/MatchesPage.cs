using COSC295assign2Cao7992.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace COSC295assign2Cao7992.Views
{
    public class MatchesPage : ContentPage
    {
        private static readonly int ListViewHeight = 800;
        private static Database mDatabase;
        private static int mOpponentId;
        private readonly ListView mListView;
        private readonly TableView mTableView;

        private readonly Picker tbGamePicker;
        private readonly DatePicker tbDatePicker;
        private readonly EntryCell tbCommentEntryCell;
        private readonly SwitchCell tbWinSwitchCell;
        private readonly Button btnAddMatch;

        private static ObservableCollection<Match> lstMatches = new ObservableCollection<Match>();  
        private static ObservableCollection<Game> lstGames = new ObservableCollection<Game>();
        private Match curSelectedMatch { get; set; }
        private int preSelectedGameIndex = 1;

        public MatchesPage(Database database, int opponentId)
        {
            mDatabase = database;
            mOpponentId = opponentId;
            Title = "Matches";

            // Fetch Matches and Games from database
            lstGames = new ObservableCollection<Game>(mDatabase.GetGames());

            // ListView for displaying matches
            mListView = new ListView
            {
                ItemsSource = lstMatches,
                RowHeight = MatchCell.MatchCellHeight,
                ItemTemplate = new DataTemplate(typeof(MatchCell)),
                VerticalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = ListViewHeight,
            };

            // Initialize current selected Matche
            curSelectedMatch = (Match)mListView.SelectedItem;

            mListView.ItemTapped += (sender, e) =>
            {
                if ((Match)mListView.SelectedItem != curSelectedMatch)
                {
                    curSelectedMatch = (Match)mListView.SelectedItem;
                }
                else
                {
                    mListView.SelectedItem = curSelectedMatch = null;
                }
                RefreshTableView();
                RefreshButtonView();
            };

            GetPreviousSelectedGame();
            // Picker for selecting a game
            tbGamePicker = new Picker
            {
                Title = "Select Game",
                ItemsSource = lstGames,
                ItemDisplayBinding = new Binding("GameName"),
                SelectedItem = lstGames.ElementAt(preSelectedGameIndex - 1),
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            ViewCell cellGamePicker = new ViewCell 
            { 
                View = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 2,
                    Padding = new Thickness(20,0,0,0),
                    Children = {
                        new Label { Text = "Game: ", VerticalOptions = LayoutOptions.Center},
                        tbGamePicker 
                    }
                }
            };

            // Date Picker
            tbDatePicker = new DatePicker { Date = DateTime.Today, Format = "dddd, MMMM dd, yyyy", HorizontalOptions = LayoutOptions.FillAndExpand };
            ViewCell cellDatePicker = new ViewCell 
            {
                View = new StackLayout
                { 
                    Orientation = StackOrientation.Horizontal,
                    Spacing=2,
                    Padding = new Thickness (20,0,0,0),
                    Children = {
                        new Label { Text = "Date: ", VerticalOptions = LayoutOptions.Center},
                        tbDatePicker, 
                    }
                }
            };

            // Comment 
            tbCommentEntryCell = new EntryCell { Label = "Comment: " };

            // Win Switch
            tbWinSwitchCell = new SwitchCell { Text = "Win? ", On = false};

            // Add Button at the end of table
            btnAddMatch = new Button { Text = "Add" };
            btnAddMatch.Clicked += SaveMatch;

            mTableView = new TableView
            {
                Intent = TableIntent.Form,
                Root = new TableRoot
                {
                    new TableSection("Add Match")
                    {
                        cellDatePicker, tbCommentEntryCell, cellGamePicker, tbWinSwitchCell,
                    }
                },
                VerticalOptions = LayoutOptions.End,
            };

            // Layout
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Padding = 20,
                Children =
                {
                    mListView,
                    mTableView,
                    btnAddMatch
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (mDatabase.GetOpponent(mOpponentId) == null)
            {
                Navigation.PopToRootAsync();
            }
            RefreshMatchesList();
        }

        protected async void GetPreviousSelectedGame()
        {
            string result = await SecureStorage.GetAsync("preSelectedGame");
            if (result == null)
            {
                await SecureStorage.SetAsync("preSelectedGame", "1");
            }
            else
            {
                preSelectedGameIndex = int.Parse(result);
            }
        }

        private static void RefreshMatchesList()
        {
            var matchesFromDb = mDatabase.GetMatchesByOpponent(mOpponentId);
            lstMatches.Clear(); // Clear old data
            foreach (Match match in matchesFromDb)
            {
                lstMatches.Add(match); // Add updated data
            }
        }

        private void RefreshTableView()
        {
            if (curSelectedMatch != null)
            {
                tbGamePicker.SelectedItem = lstGames.ElementAt(curSelectedMatch.GameID-1);
                tbDatePicker.Date = curSelectedMatch.Date;
                tbCommentEntryCell.Text = curSelectedMatch.Comment;
                tbWinSwitchCell.On = curSelectedMatch.Win;
            }
            else
            {
                tbGamePicker.SelectedItem = lstGames.ElementAt(preSelectedGameIndex - 1);
                tbDatePicker.Date = DateTime.Today;
                tbCommentEntryCell.Text = "";
                tbWinSwitchCell.On = false;
            }
        }

        private void RefreshButtonView()
        {
            btnAddMatch.Text = (curSelectedMatch==null) ? "Add" : "Update";
        }

        private void SaveMatch(object sender, EventArgs e)
        {
            if (tbGamePicker.SelectedIndex < 0) return;

            var match = new Match
            {
                OpponentID = mOpponentId,
                GameID = ((Game)tbGamePicker.SelectedItem).ID,
                Date = tbDatePicker.Date,
                Comment = tbCommentEntryCell.Text,
                Win = tbWinSwitchCell.On
            };
            if (curSelectedMatch == null)
            {
                mDatabase.SaveMatch(match);
            }
            else
            {
                match.ID = curSelectedMatch.ID;
                mDatabase.UpdateMatch(match);
            }
            curSelectedMatch = null;
            preSelectedGameIndex = tbGamePicker.SelectedIndex + 1;
            RefreshMatchesList();
            RefreshTableView();
            RefreshButtonView();
            SecureStorage.SetAsync("preSelectedGame", preSelectedGameIndex.ToString());
        }

        private static void DeleteMatch(Match match)
        {
            if (match == null) return;
            mDatabase.DeleteMatch(match);
            RefreshMatchesList();
        }

        internal class MatchCell : ViewCell
        {
            public const int MatchCellHeight = 90;
            public MatchCell()
            {
                Label lblOpponentName = new Label { FontAttributes = FontAttributes.Bold };
                Label lblDate = new Label { FontAttributes = FontAttributes.Italic, TextColor = Color.Gray, HorizontalOptions = LayoutOptions.StartAndExpand };
                Label lblComment = new Label { FontAttributes = FontAttributes.Italic, TextColor = Color.Gray, HorizontalOptions = LayoutOptions.End };
                Label lblGame = new Label { TextColor = Color.Brown, HorizontalOptions = LayoutOptions.StartAndExpand };
                Label lblWin = new Label { Text = "Win? ", HorizontalTextAlignment = TextAlignment.End };
                Switch swWin = new Switch { IsEnabled = false, HorizontalOptions = LayoutOptions.End };

                lblOpponentName.SetBinding(Label.TextProperty, "OpponentID", converter: new OpponentIdConvert());
                lblDate.SetBinding(Label.TextProperty, "Date", stringFormat: "{0:dddd, MMMM dd, yyyy}");
                lblComment.SetBinding(Label.TextProperty, "Comment");
                lblGame.SetBinding(Label.TextProperty, "GameID", converter: new GameIdConvert());
                swWin.SetBinding(Switch.IsToggledProperty, "Win");

                // Add a trigger to change the color based on the IsToggled property
                swWin.Toggled += (sender, e) =>
                {
                    if (swWin.IsToggled)
                    {
                        swWin.BackgroundColor = Color.Pink;
                    }
                    else
                    {
                        swWin.BackgroundColor = Color.Gray;
                    }
                };

                StackLayout stack1 = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { lblDate, lblComment }
                };

                StackLayout stack2 = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = {lblGame, lblWin, swWin }
                };

                View = new StackLayout
                {
                    Spacing = 5,
                    Padding = 5,
                    Children = { lblOpponentName, stack1, stack2 },
                    Orientation = StackOrientation.Vertical,
                };

                MenuItem mi = new MenuItem
                {
                    Text = "Delete",
                    IsDestructive = true,
                };
                mi.Clicked += (sender, e) =>
                {
                    MatchesPage.DeleteMatch((Match)this.BindingContext);
                };
                ContextActions.Add(mi);
            }

            public class GameIdConvert : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    return MatchesPage.mDatabase.GetGame(int.Parse(value.ToString())).GameName;
                }
                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    throw new NotImplementedException();
                }
            }
            public class OpponentIdConvert : IValueConverter
            {
                public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    return MatchesPage.mDatabase.GetOpponent(int.Parse(value.ToString())).FullName;
                }
                public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
