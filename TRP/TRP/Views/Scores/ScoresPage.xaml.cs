using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;
using TRP.ViewModels;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoresPage : ContentPage
    {
        private ScoresViewModel _viewModel;

        public ScoresPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = ScoresViewModel.Instance;
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var data = args.SelectedItem as Score;
            if (data == null)
                return;

            await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(data)));

            // Manually deselect item.
            ScoreListView.SelectedItem = null;
        }

        private async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoreNewPage());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;

            if (ToolbarItems.Count > 0)
            {
                ToolbarItems.RemoveAt(0);
            }

            InitializeComponent();

            if (_viewModel.Dataset.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }

            BindingContext = _viewModel;
        }
    }
}