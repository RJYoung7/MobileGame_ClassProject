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
        private CharactersViewModel _viewModel; // View model for this page

        // Constructor: create page and load view model
        public GameOverPage ()
		{
			InitializeComponent ();

            BindingContext = _viewModel = CharactersViewModel.Instance;
        }

        // When this button is clicked, add new Scores page onto stack
        private async void HighScoreButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoresPage());
        }

        // When this button is clicked, create new Battle Begin (manual battle page) and add to stack 
        private async void HomeButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BattleBeginPage());
        }
    }
}