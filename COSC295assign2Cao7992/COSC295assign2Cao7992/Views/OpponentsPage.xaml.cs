using Xamarin.Forms;
using System.Collections.ObjectModel;
using COSC295assign2Cao7992.Models;
using System;

namespace COSC295assign2Cao7992.Views
{
    public partial class OpponentsPage : ContentPage
    {
        private ListView _listView;
        private Database _database;
        private ObservableCollection<Opponent> lstOpponents = new ObservableCollection<Opponent>();

        public OpponentsPage(Database database)
        {
            _database = database;
            Title = "Opponents";

            //lstOpponents = new ObservableCollection<Opponent>(_database.GetOpponents());
            _listView = new ListView { ItemsSource = lstOpponents };
            _listView.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    var opponent = (Opponent)e.SelectedItem;
                    Navigation.PushAsync(new MatchesPage(_database, opponent.ID));
                }
            };

            var addButton = new Button { Text = "Add New Opponent" };
            addButton.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new AddOpponentPage(_database));
            };

            Content = new StackLayout { Children = { _listView, addButton } };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshOpponentsList();
        }

        private void RefreshOpponentsList()
        {
            var opponentsFromDb = _database.GetOpponents();
            lstOpponents.Clear(); // Clear old data
            foreach (var opponent in opponentsFromDb)
            {
                lstOpponents.Add(opponent); // Add updated data
            }
        }
    }

    public class AddOpponentPage : ContentPage
    {
        private Entry _firstNameEntry, _lastNameEntry, _addressEntry, _phoneEntry, _emailEntry;
        private Database _database;

        public AddOpponentPage(Database database)
        {
            _database = database;
            Title = "Add Opponent";

            _firstNameEntry = new Entry { Placeholder = "First Name" };
            _lastNameEntry = new Entry { Placeholder = "Last Name" };
            _addressEntry = new Entry { Placeholder = "Address" };
            _phoneEntry = new Entry { Placeholder = "Phone" };
            _emailEntry = new Entry { Placeholder = "Email" };

            var saveButton = new Button { Text = "Save" };
            saveButton.Clicked += async (sender, e) =>
            {
                var opponent = new Opponent
                {
                    FirstName = _firstNameEntry.Text,
                    LastName = _lastNameEntry.Text,
                    Address = _addressEntry.Text,
                    Phone = _phoneEntry.Text,
                    Email = _emailEntry.Text
                };
                _database.SaveOpponent(opponent);
                await Navigation.PopAsync();
            };

            Content = new StackLayout
            {
                Padding = 10,
                Children = { _firstNameEntry, _lastNameEntry, _addressEntry, _phoneEntry, _emailEntry, saveButton }
            };
        }
    }

    

    public class GamesPage : ContentPage
    {
        public GamesPage(Database database)
        {
            Title = "Games";
            var listView = new ListView { ItemsSource = database.GetGames() };
            Content = new StackLayout { Children = { listView } };
        }
    }

    public class SettingsPage : ContentPage
    {
        public SettingsPage(Database database)
        {
            Title = "Settings";
            var resetButton = new Button { Text = "Reset App" };
            resetButton.Clicked += (sender, e) => { database.ResetDatabase(); };
            Content = new StackLayout { Children = { new Label { Text = "Reset all data." }, resetButton } };
        }
    }

    public class AppShell : Shell
    {
        public AppShell(Database database)
        {
            Items.Add(new ShellContent { Title = "Opponents", Content = new OpponentsPage(database) });
            Items.Add(new ShellContent { Title = "Games", Content = new GamesPage(database) });
            Items.Add(new ShellContent { Title = "Settings", Content = new SettingsPage(database) });
        }
    }
}
