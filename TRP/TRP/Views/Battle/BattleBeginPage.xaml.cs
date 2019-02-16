using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BattleBeginPage : ContentPage
	{
		public BattleBeginPage ()
		{
			InitializeComponent ();
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
            await Navigation.PushAsync(new BattlePage());
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

    }
}