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
    }
}