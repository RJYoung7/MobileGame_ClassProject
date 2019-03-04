using System;
using TRP.GameEngine;
using TRP.ViewModels;
using TRP.Views;   

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Views.Battle;

namespace TRP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OpeningPage : ContentPage
	{
        // Constructor: initialize page 
		public OpeningPage ()
		{
			InitializeComponent();
		}

        // When this button is clicked, run auto battle and return results 
        private async void AutoBattleButton_Command(object sender, EventArgs e)
        {
            // Can create a new battle engine...
            var myBattleEngine = new AutoBattleEngine();

            // Add characters to AutoBattle
            //var result = myBattleEngine.AddCharactersToBattle();

            // Start AutoBattle
            myBattleEngine.RunAutoBattle();

            // Display result of AutoBattle
            await DisplayAlert(null, myBattleEngine.GetResultOutput(), null, "Next");

            // String to hold score.
            var outputString = "Score: " + myBattleEngine.GetScoreValue();

            // Show player their score, and allow option to view more details.
            var action = await DisplayActionSheet(outputString, "Cancel", null, "View Score");

            // If user wants to view more score details, take them there.
            if (action == "View Score")
            {
                await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(myBattleEngine.GetScoreObject())));
            }
        }

        // When this button is clicked, create Battle page (for manual battle) and add onto stack
        private async void ManualBattleButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BattleBeginPage());
        }

        // When this button is clicked, create About page and add onto stack
        private async void AboutButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}