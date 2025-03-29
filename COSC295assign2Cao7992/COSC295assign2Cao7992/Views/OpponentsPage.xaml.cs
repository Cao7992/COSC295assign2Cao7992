using Xamarin.Forms;
using System.Collections.ObjectModel;
using OpponentTracker.Models;
using OpponentTracker.Data;
using COSC295assign2Cao7992.Models;
using System;

namespace OpponentTracker.Views
{
    public partial class OpponentsPage : ContentPage
    {
        private readonly DatabaseHelper _databaseHelper = new DatabaseHelper();
        private ObservableCollection<Opponent> _opponents;

        public OpponentsPage()
        {
            InitializeComponent();
            LoadOpponents();
        }

        private async void LoadOpponents()
        {
            var opponents = await _databaseHelper.GetOpponentsAsync();
            _opponents = new ObservableCollection<Opponent>(opponents);
            OpponentsListView.ItemsSource = _opponents;
        }

        private async void AddOpponentClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOpponentPage());
        }

        private async void OnOpponentSelected(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Opponent selectedOpponent)
            {
                await Navigation.PushAsync(new MatchesPage(selectedOpponent.ID));
            }
        }
    }
}
