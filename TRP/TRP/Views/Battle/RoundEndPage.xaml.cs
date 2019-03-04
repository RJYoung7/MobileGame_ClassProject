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
            await Navigation.PushAsync(new BattlePage());
        }

    }
}