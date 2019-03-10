using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;
using TRP.ViewModels;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundEndPage : ContentPage
	{
        private CharactersViewModel _viewModel; // View model for this page

        public RoundEndPage ()
		{
			InitializeComponent ();
            BindingContext = _viewModel = CharactersViewModel.Instance;

        }

        private async void ItemPickupButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ItemPickupPage());
        }

        private async void NextRoundButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BattlePage(BattleViewModel.Instance));
        }

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