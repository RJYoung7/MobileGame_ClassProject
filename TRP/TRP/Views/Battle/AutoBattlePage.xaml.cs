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
            var myBattleEngine = new AutoBattleEngine();

            var result = myBattleEngine.AddCharactersToBattle();

            //var charactersOutput = "";
            //if (result)
            //{
            //    charactersOutput += "Count: ";
            //    charactersOutput += myBattleEngine.CharacterList.Count + "\n";
            //    foreach (var ch in myBattleEngine.CharacterList)
            //    {
            //        charactersOutput += ch.FormatOutput() + "\n";
            //    }
            //    await DisplayAlert("Chosen characters", charactersOutput, "OK");
            //}

            //if (result == false)
            //{
            //    var answer = await DisplayAlert("Error", "No Characters to battle with", "OK","Cancel");
            //    if (answer)
            //    {
            //        var a = 1;
            //        // Can't run auto battle, no characters...
            //    }
            //}

            myBattleEngine.RunAutoBattle();

            if (myBattleEngine.BattleScore.RoundCount < 1)
            {
                var answer = await DisplayAlert("Error", "No Rounds Fought", "OK", "Cancel");
                if (answer)
                {
                    var a = 1;
                    // Can't run auto battle, no characters...
                }
            }

            var outputString = "Battle Over! Socre: " + myBattleEngine.BattleScore.ScoreTotal;
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