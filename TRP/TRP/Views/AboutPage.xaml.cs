
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
        public AboutPage()
        {
            InitializeComponent();

            SettingDataSource.IsToggled = true;
            // Set the flag for Mock on or off...
            //UseMockDataSource.IsToggled = (MasterDataStore.GetDataStoreMockFlag() == DataStoreEnum.Mock);
            //SetDataSource(UseMockDataSource.IsToggled);

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


            var myTestItem = new Item();
            var myTestCharacter = new Character();
            var myTestMonster = new Monster();

            var myOutputItem = myTestItem.FormatOutput();
            var myOutputCharacter = myTestCharacter.FormatOutput();
            var myOutputMonster = myTestMonster.FormatOutput();

        }
        private void Switch_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            SetDataSource(SettingDataSource.IsToggled);
        }

        private void SetDataSource(bool isMock)
        {
            var set = DataStoreEnum.Sql;

            if (isMock)
            {
                set = DataStoreEnum.Mock;
            }

            MasterDataStore.ToggleDataStore(set);
        }

        private void EnableDebugSettings_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.

            DebugSettingsFrame.IsVisible = (e.Value);
        }

        private void UseMockDataSourceSwitch_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            SetDataSource(e.Value);
        }

        // Debug Switches

       
        // Turn on Critical Misses
        private void EnableCriticalMissProblems_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            GameGlobals.EnableCriticalMissProblems = e.Value;
        }

        // Turn on Critical Hit Damage
        private void EnableCriticalHitDamage_OnToggled(object sender, ToggledEventArgs e)
        {
            // This will change out the DataStore to be the Mock Store if toggled on, or the SQL if off.
            GameGlobals.EnableCriticalHitDamage = e.Value;
        }
                
        private async void ClearDatabase_Command(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete", "Sure you want to Delete All Data, and start over?", "Yes", "No");
            if (answer)
            {
                // Call to the SQL DataStore and have it clear the tables.
                SQLDataStore.Instance.InitializeDatabaseNewTables();
            }
        }


        // Add code for GetItems_Command
        // add your code here

        private async void GetItems_Command(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Get", "Sure you want to Get Items from the Server?", "Yes", "No");
            if (answer)
            {
                // Call to the Item Service and have it Get the Items
                
            }
        }

        private async void GetItemsPost_Command(object sender, EventArgs e)
        {
            //ItemsController.Instance.GetItemsFromGame(int number, int level, AttributeEnum attribute, ItemLocationEnum location, bool random, bool updateDataBase)

            var number = 10;    // 10 items
            var level = 6;  // Max Value of 6
            var attribute = AttributeEnum.Unknown;  // Any Attribute
            var location = ItemLocationEnum.Unknown;    // Any Location
            var random = true;  // Random between 1 and Level
            var updateDataBase = true;  // Add them to the DB

            // GetItemsFromGame(1,10,Speed,Feet,false,true) will return shoes value 10 of speed.

            var myDataList = new List<Item>();

            var myOutput = "No Results";

            if (myDataList != null && myDataList.Count > 0)
            {
                // Reset the output
                myOutput = "";

                foreach (var item in myDataList)
                {
                    // Add them line by one, use \n to force new line for output display.
                    myOutput += item.FormatOutput() + "\n";
                }
            }

            var answer = await DisplayAlert("Returned List", myOutput, "Yes", "No");
        }

    }
}