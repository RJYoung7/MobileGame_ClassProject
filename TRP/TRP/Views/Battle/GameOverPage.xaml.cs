using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.ViewModels;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GameOverPage : ContentPage
	{
        private BattleViewModel _viewModel; // View model for this page

        // Constructor: create page and load view model
        public GameOverPage ()
		{
			InitializeComponent ();

            BindingContext = _viewModel = BattleViewModel.Instance;
            battleNum.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.BattleNumber);
            numRounds.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.RoundCount);
            numMonstersKilled.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.MonsterSlainNumber);
            finalScore.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.ScoreTotal);
        }

        // When this button is clicked, add new Scores page onto stack
        private async void HighScoreButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoresPage());
        }

        // When this button is clicked, create new Battle Begin (manual battle page) and add to stack 
        private async void HomeButton_Clicked(object sender, EventArgs e)
        {
            _viewModel.BattleEngine.BattleEngineClearData();
            await Navigation.PushAsync(new BattleBeginPage());
        }
    }
}