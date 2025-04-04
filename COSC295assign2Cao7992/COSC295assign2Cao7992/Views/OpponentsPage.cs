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
        // create 2 private variables that is used for this page - database to access database source, lstOpponent - linked the item source of a list view
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

            // handle clicking event on each item of a listview, leading to new MatchesPage of that Opponent
            listView.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    Opponent opponent = (Opponent)e.SelectedItem;
                    Navigation.PushAsync(new MatchesPage(_database, opponent.ID));
                }
            };

            // initialize the Add Button
            Button btnAddOpponent = new Button
            {
                Text = "Add New Opponent",
                TextColor = Color.Red,
                BorderColor = Color.Red,
                Margin = new Thickness(40, 40, 40, 20)
            };
            // handle clicking event on the Add button
            btnAddOpponent.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new AddOpponentPage(_database));
            };

            // create a header tag, making the list view look like a 2-columns table
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

            // display this page as Stack, which contain header at top, list of opponent after that and finally a Add button, vertically
            Content = new StackLayout { Children = { header, listView, btnAddOpponent } };
        }
        // method to handle action when the page is reloaded/ returned
        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshOpponentsList();
        }
        /*
         * helper method to update any change from database into the listView Item resource
         */
        private static void RefreshOpponentsList()
        {
            var opponentsFromDb = _database.GetOpponents();
            lstOpponents.Clear(); // Clear old data
            foreach (var opponent in opponentsFromDb)
            {
                lstOpponents.Add(opponent); // Add updated data
            }
        }
        /*
         * Helper method to delete specified Opponent from the database
         */
        public static void DeleteOpponent(Opponent op)
        {
            _database.DeleteOpponent(op.ID);
            RefreshOpponentsList();
        }
    }

    /*
     * Helper Class: 
     * Purpose: to layout each cell in the list View
     * Conponent include: height property, FullName - computed property, PhoneNumnber property of  Opponent object
     */
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
            
            // Call Menu item when Item is long-tapped, generating delete option
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
}