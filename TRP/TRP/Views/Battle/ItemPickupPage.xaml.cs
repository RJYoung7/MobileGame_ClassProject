using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.ViewModels;
using TRP.Models;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemPickupPage : ContentPage
	{
        private BattleViewModel _viewModel; // View model for this page

        // Constructor: initialize the page 
        public ItemPickupPage ()
		{
			InitializeComponent ();

            BindingContext = _viewModel = BattleViewModel.Instance;
        }

        // When item is selected, should go to items detail page
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Item;
            if (data == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(data)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }
    }


}