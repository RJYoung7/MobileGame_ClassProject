using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using TRP.GameEngine;
using TRP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AutoBattlePage : ContentPage
	{
		public AutoBattlePage ()
		{
			InitializeComponent ();
		}

        private async void AutoBattleButton_Command(object sender, EventArgs e)
        {
            // Can create a new battle engine...
            var myBattleEngine = new AutoBattleEngine();

            // Add characters to AutoBattle
            var result = myBattleEngine.AddCharactersToBattle();

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
    }
}