using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var myBattleEngine = new BattleEngine();

            var result = myBattleEngine.AutoBattle();

            if (result == false)
            {
                var answer = await DisplayAlert("Error", "No Characters to battle with", "OK","Cancel");
                if (answer)
                {
                    var a = 1;
                    // Can't run auto battle, no characters...
                }
            }

            if (myBattleEngine.BattleScore.RoundCount < 1)
            {
                var answer = await DisplayAlert("Error", "No Rounds Fought", "OK", "Cancel");
                if (answer)
                {
                    var a = 1;
                    // Can't run auto battle, no characters...
                }
            }

            var outputString = "Battle Over! Score " + myBattleEngine.BattleScore.ScoreTotal;
            var action = await DisplayActionSheet(outputString, 
                "Cancel", 
                null, 
                "View Score");
            if (action == "View Score")
            {
                await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(myBattleEngine.BattleScore)));
            }
        }
    }
}