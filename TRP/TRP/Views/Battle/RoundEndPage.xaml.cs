using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;
using TRP.ViewModels;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundEndPage : ContentPage
    {
        private BattleViewModel _viewModel;

        // Constructor: initialize the page
        public RoundEndPage(BattleViewModel battleViewModel)
		{
			InitializeComponent ();
            BindingContext = _viewModel = BattleViewModel.Instance;
            numRoundsText.Text = "Success! Round " + Convert.ToString(battleViewModel.BattleEngine.BattleScore.RoundCount) + " Cleared";
           
        }

        // Button to go to page that displays items picked up in round
        private async void ItemPickupButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemPickupPage());
        }

        // Button for next round in game
        private async void NextRoundButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BattlePage(BattleViewModel.Instance));
        }

        // When character is selected, should go to details of character
        private async void OnCharacterSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Character;
            if (data == null)
                return;

            await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(data)));

            // Manually deselect item.
            CharactersListView.SelectedItem = null;
        }

    }
}