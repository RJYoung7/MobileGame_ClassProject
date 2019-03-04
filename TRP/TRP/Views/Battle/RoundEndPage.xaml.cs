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
	public partial class RoundEndPage : ContentPage
	{
		public RoundEndPage ()
		{
			InitializeComponent ();
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