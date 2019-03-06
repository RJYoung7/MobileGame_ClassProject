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
	public partial class ItemPickupPage : ContentPage
	{

        private ItemsViewModel _viewModel; // View model for this page

        public ItemPickupPage ()
		{
			InitializeComponent ();

            BindingContext = _viewModel = ItemsViewModel.Instance;
        }
	}
}