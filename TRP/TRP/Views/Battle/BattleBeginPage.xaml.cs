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

        public BattleBeginPage ()
		{
			InitializeComponent ();
            BindingContext = _viewModel = BattleViewModel.Instance;
		}

        private async void SelectCharactersButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharactersSelectPage());
        }

        private async void SelectMonstersButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonstersSelectPage());
        }

        private async void SelectStartButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BattlePage(BattleViewModel.Instance));
        }

        // Remove after BattleEngine hookup
        private async void ItemPickupButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemPickupPage());
        }

        // Remove after BattleEngine hoockup
        private async void RoundEndButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RoundEndPage());
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GameOverPage());
        }
    }
}