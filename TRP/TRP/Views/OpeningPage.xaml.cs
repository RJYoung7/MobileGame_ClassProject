using System;

using TRP.Views;   

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Views.Battle;

namespace TRP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OpeningPage : ContentPage
	{
		public OpeningPage ()
		{
			InitializeComponent ();
		}

        private async void AutoBattleButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AutoBattlePage());
        }

        private async void ManualBattleButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BattleBeginPage());
        }

        private async void AboutButton_Command(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }


    }
}