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
	public partial class BattleBeginPage : ContentPage
	{
        private BattleViewModel _viewModel; // View model for this page

        // Constructor: load view model
        public BattleBeginPage ()
		{
			InitializeComponent ();
            BindingContext = _viewModel = BattleViewModel.Instance;
		}

        // When this button is clicked, create SelectCharacters page and add onto stack
        private async void SelectCharactersButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharactersSelectPage());
        }

        // When this button is clicked, create Battle page (with no info for now)
        private async void SelectStartButton_Command(object sender, EventArgs e)
        {
            _viewModel.StartRound();
            await Navigation.PushAsync(new BattlePage(BattleViewModel.Instance));
        }
    }
}