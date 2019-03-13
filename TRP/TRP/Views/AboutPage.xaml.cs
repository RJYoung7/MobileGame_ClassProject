using System;
using TRP.Services;
using TRP.Controllers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TRP.ViewModels;
using TRP.Models;
using System.Collections.Generic;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        private AboutViewModel _viewModel = new AboutViewModel(); // view model for page

        // Constructor: bind view model to page 
        public AboutPage()
        {
            InitializeComponent();

            BindingContext = _viewModel;

            // Datastore settigngs
            UseMockDataSource.IsToggled = (MasterDataStore.GetDataStoreMockFlag() == DataStoreEnum.Mock);
            SetDataSource(UseMockDataSource.IsToggled);

            // Example of how to add an view to an existing set of XAML. 
            // Give the Xaml layout you want to add the data to a good x:Name, so you can access it.  Here "DateRoot" is what I am using.
            var dateLabel = new Label
            {
                Text = DateTime.Now.ToShortDateString(),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = "Helvetica Neue",
                FontAttributes = FontAttributes.Bold,
                FontSize = 12,
                TextColor = Color.Black,
            };

            DateRoot.Children.Add(dateLabel);

            // Set debug settings
            EnableCriticalMissProblems.IsToggled = GameGlobals.EnableCriticalMissProblems;
            EnableCriticalHitDamage.IsToggled = GameGlobals.EnableCriticalHitDamage;
            EnableMonsterStolenItem.IsToggled = GameGlobals.EnableMonsterStolenItem;

            // Turn off the Debug Frame
            DebugSettingsFrame.IsVisible = false;

            // Turn off Forced Random Numbers Frame
            ForcedRandomValuesSettingsFrame.IsVisible = false;

            // Turn off Database Settings Frame
            DatabaseSettingsFrame.IsVisible = false;

            var myTestItem = new Item();
            var myTestCharacter = new Character();
            var myTestMonster = new Monster();

            var myOutputItem = myTestItem.FormatOutput();
            var myOutputCharacter = myTestCharacter.FormatOutput();
            var myOutputMonster = myTestMonster.FormatOutput();

        }

        // Set datastore based on user's toggle 
        private void SetDataSource(bool isMock)
        {
            var set = DataStoreEnum.SQL;

            if (isMock)
            {
                set = DataStoreEnum.Mock;
            }

            MasterDataStore.ToggleDataStore(set);
        }

        // Enable or disable debug settings 
        private void EnableDebugSettings_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.

            DebugSettingsFrame.IsVisible = (e.Value);
        }

        // Enable or disable database settings 
        private void DatabaseSettingsSwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            DatabaseSettingsFrame.IsVisible = (e.Value);
        }

        private void UseMockDataSourceSwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            SetDataSource(e.Value);
        }

        // Debug Switches

        // Turn on forced random values 
        private void UseForcedRandomValuesSwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                ForcedRandomValuesSettingsFrame.IsVisible = true;
                GameGlobals.EnableRandomValues();

                GameGlobals.SetForcedRandomNumbersValue(Convert.ToInt16(ForcedValue.Text));
                ForcedHitValue.Text = GameGlobals.ForceToHitValue.ToString();
                forcehit.Value = GameGlobals.ForceToHitValue;
            }
            else
            {
                GameGlobals.DisableRandomValues();
                ForcedRandomValuesSettingsFrame.IsVisible = false;
            }
        }

        // The stepper function for Forced Value
        private void ForcedValue_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            ForcedValue.Text = String.Format("{0}", e.NewValue);
            GameGlobals.SetForcedRandomNumbersValue(Convert.ToInt16(ForcedValue.Text));
        }

        // The stepper function for To Force To Hit Value
        private void ForcedHitValue_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            ForcedHitValue.Text = String.Format("{0}", e.NewValue);
            GameGlobals.SetForcedHitValue(Convert.ToInt16(ForcedHitValue.Text));
        }

        // Turn on Critical Misses
        private void EnableCriticalMissProblems_OnToggled(object sender, ToggledEventArgs e)
        {
            GameGlobals.EnableCriticalMissProblems = e.Value;
        }

        // Turn on Critical Hit Damage
        private void EnableCriticalHitDamage_OnToggled(object sender, ToggledEventArgs e)
        {
            GameGlobals.EnableCriticalHitDamage = e.Value;
        }
         
        // A dialog box to confirm clearing of database 
        private async void ClearDatabase_Command(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete", "Sure you want to Delete All Data, and start over?", "Yes", "No");
            if (answer)
            {
                // Call to the SQL DataStore and have it clear the tables.
                SQLDataStore.Instance.InitializeDatabaseNewTables();
                MockDataStore.Instance.InitializeDatabaseNewTables();
            }
        }

        #region GetPostCommands
        /// <summary>
        /// The GetItems_Command will use a GET call to retrieve items from the server
        /// </summary>
        private async void GetItems_Command(object sender, EventArgs e)
        {
            var myOutput = "No results";
            var myDataList = new List<Item>();

            // Dialog box to confirm the GET call
            var answer = await DisplayAlert("Get", "Sure you want to Get Items from the Server?", "Yes", "No");
            if (answer)
            {
                // Call to the Item Service and have it Get the Items
                var numItemsToGet = Convert.ToInt32(ServerItemValue.Text);
                myDataList = await ItemsController.Instance.GetItemsFromServer(numItemsToGet);
                if (myDataList != null && myDataList.Count > 0)
                {
                    // Reset the output
                    myOutput = "";

                    foreach (var item in myDataList)
                    { 
                        // Add them line by one, use \n to force new line for output display.
                        // Build up the output string by adding formatted Item Output
                        myOutput += item.FormatOutput() + "\n\n";
                    }
                }
            }

            // Display a list of items return with details in a dialog box
            await DisplayAlert("Returned List", myOutput, "OK");
        }

        /// <summary>
        /// Get items from the server using a POST call.  Will get items based
        /// on parameters assigned.
        /// </summary>
        private async void GetItemsPost_Command(object sender, EventArgs e)
        {
            var myOutput = "No Results";
            var myDataList = new List<Item>();

            var number = Convert.ToInt32(ServerItemValue.Text);
            var level = 6;  // Max Value of 6
            var attribute = AttributeEnum.Unknown;  // Any Attribute
            var location = ItemLocationEnum.Unknown;    // Any Location
            var random = true;  // Random between 1 and Level
            var updateDataBase = true;  // Add them to the DB

            // will return shoes value 10 of speed.
            // Example  result = await ItemsController.Instance.GetItemsFromGame(1, 10, AttributeEnum.Speed, ItemLocationEnum.Feet, false, true);
            //ItemsController.Instance.GetItemsFromGame(int number, int level, AttributeEnum attribute, ItemLocationEnum location, bool random, bool updateDataBase)

            // Display a confirmation dialog box to confirm the user still wants to get the items
            var answer = await DisplayAlert("Post", "Sure you want to Post Items from the Server?", "Yes", "No");
            if (answer)
            {
                // Get the items and await the call
                myDataList = await ItemsController.Instance.GetItemsFromGame(number, level, attribute, location, random, updateDataBase);

                // Check if the post returned any items
                if (myDataList != null && myDataList.Count > 0)
                {
                    // Reset the output
                    myOutput = "";

                    foreach (var item in myDataList)
                    {
                        // Add them line by one, use \n to force new line for output display.
                        myOutput += item.FormatOutput() + "\n\n";
                    }
                }

                // Display the list of items return with details in a dialog box
                await DisplayAlert("Returned List", myOutput, "OK");
            }
        }
        #endregion

        // Turn on feature to enable a monster to have chance to steal dropped item from character
        private void EnableStolenItem_OnToggled(object sender, ToggledEventArgs e)
        {
            GameGlobals.EnableMonsterStolenItem = e.Value;
        }

        // Turn on feature to enable a monster to have chance to steal dropped item from character
        private void EnableMiracleMax_OnToggled(object sender, ToggledEventArgs e)
        {
            GameGlobals.EnableRevivalOnce = e.Value;
        }

    }
}