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
	public partial class BattlePage : ContentPage
	{
		public BattlePage ()
		{
			InitializeComponent ();
		}

        private async void AttackButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert(null, "Attacked", null, "Next");

        }
    }
}