using COSC295assign2Cao7992.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace COSC295assign2Cao7992.Views
{
    public class AddOpponentPage : ContentPage
    {
        // Create private variables for this page to store Opponent Object that we want to create
        private EntryCell cellFName, cellLName, cellAddress, cellPhone, cellEmail;
        private static Database _database;

        public AddOpponentPage(Database database)
        {
            _database = database;
            Title = "AddOpponentsPage";

            // Initialize each components
            cellFName = new EntryCell { Label = "First Name: ", LabelColor = Color.Brown, Placeholder = "required", };
            cellLName = new EntryCell { Label = "Last Name: ", LabelColor = Color.Brown, Placeholder = "required", };
            cellAddress = new EntryCell { Label = "Address: ", LabelColor = Color.Brown };
            cellPhone = new EntryCell { Label = "Phone: ", LabelColor = Color.Brown , Placeholder = "required" };
            cellEmail = new EntryCell { Label = "Email: ", LabelColor = Color.Brown };

            Button btnSave = new Button { Text = "Save" };
            // handle clicking event of Save button
            btnSave.Clicked += async (sender, e) =>
            {
                // Make sure required fields are not null nor blank before proceeding saving process
                if (cellFName.Text == null || cellLName.Text == null || cellPhone.Text == null)  return; 
                if (cellFName.Text.Trim() == "" || cellLName.Text.Trim() == "" || cellPhone.Text.Trim() == "")  return; 
                Opponent opponent = new Opponent
                {
                    FirstName = cellFName.Text.Trim(),
                    LastName = cellLName.Text.Trim(),
                    Address = cellAddress.Text,
                    Phone = cellPhone.Text.Trim(),
                    Email = cellEmail.Text
                };
                _database.SaveOpponent(opponent);
                await Navigation.PopAsync();
            };

            // display the Page as Stack, with table in form of a form, and a Save button just below it
            Content = new StackLayout
            {
                Spacing = 10,
                Padding = 20,
                Children =
                {
                    new TableView
                    {
                        Intent = TableIntent.Form,
                        Root = new TableRoot
                        {
                            new TableSection("Add New Opponent")
                            {
                                cellFName, cellLName, cellAddress, cellPhone, cellEmail

                            }
                        },
                    },
                    btnSave,
                },
                HeightRequest = 50,
            };
        }
    }
}