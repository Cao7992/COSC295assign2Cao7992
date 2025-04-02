using COSC295assign2Cao7992.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace COSC295assign2Cao7992.Views
{
    public class OpponentsPage : ContentPage
    {
        private static Database _database;
        private static ObservableCollection<Opponent> lstOpponents = new ObservableCollection<Opponent>();
        public OpponentsPage(Database database)
        {
            _database = database;
            Title = "Opponents";

            //lstOpponents = new ObservableCollection<Opponent>(_database.GetOpponents());
            ListView listView = new ListView 
            {
                ItemsSource = lstOpponents ,
                RowHeight = OpponentCell.OpponentCellHeight,
                ItemTemplate = new DataTemplate(typeof(OpponentCell))
            };

            listView.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    Opponent opponent = (Opponent)e.SelectedItem;
                    Navigation.PushAsync(new MatchesPage(_database, opponent.ID));
                }
            };

            Button btnAddOpponent = new Button
            {
                Text = "Add New Opponent",
                TextColor = Color.Red,
                BorderColor = Color.Red,
                Margin = new Thickness(40, 40, 40, 20)
            };
            btnAddOpponent.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new AddOpponentPage(_database));
            };

            StackLayout header = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(10),
                Children =  {     
                    new Label
                    {
                        Text = "FullName",
                        FontSize = 20,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    },
                    new Label
                    {
                        Text = "PhoneNumber",
                        FontSize = 20,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.CenterAndExpand
                    }
                }
            };

            Content = new StackLayout { Children = { header, listView, btnAddOpponent } };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshOpponentsList();
        }

        private static void RefreshOpponentsList()
        {
            var opponentsFromDb = _database.GetOpponents();
            lstOpponents.Clear(); // Clear old data
            foreach (var opponent in opponentsFromDb)
            {
                lstOpponents.Add(opponent); // Add updated data
            }
        }

        public static void DeleteOpponent(Opponent op)
        {
            _database.DeleteOpponent(op.ID);
            RefreshOpponentsList();
        }
    }

    internal class OpponentCell : ViewCell
    {
        public const int OpponentCellHeight = 50;
        public OpponentCell()
        {
            Label lblFullName = new Label 
            { 
                FontAttributes = FontAttributes.Italic, 
                HorizontalOptions = LayoutOptions.FillAndExpand, 
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center 
            };
            lblFullName.SetBinding(Label.TextProperty, "FullName");

            Label lblPhoneNumer = new Label 
            {
                FontAttributes = FontAttributes.Italic, 
                HorizontalOptions = LayoutOptions.FillAndExpand, 
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center
            };
            lblPhoneNumer.SetBinding(Label.TextProperty, "Phone");

            View = new StackLayout
            {
                Spacing = 5,
                Padding = 5,
                Children = { lblFullName, lblPhoneNumer },
                Orientation = StackOrientation.Horizontal,
            };

            MenuItem mi = new MenuItem
            {
                Text = "Delete",
                IsDestructive = true,
            };
            mi.Clicked += (sender, e) =>
            {
                OpponentsPage.DeleteOpponent((Opponent)this.BindingContext);
            };
            ContextActions.Add(mi);
        }
    }

    /*public class AppShell : Shell
    {
        public AppShell(Database database)
        {
            Items.Add(new ShellContent { Title = "Opponents", Content = new OpponentsPage(database) });
            Items.Add(new ShellContent { Title = "Games", Content = new GamesPage(database) });
            Items.Add(new ShellContent { Title = "Settings", Content = new SettingPage(database) });
        }
    }*/
}