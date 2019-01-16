using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;
using TRP.ViewModels;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoreDetailPage : ContentPage
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private ScoreDetailViewModel _viewModel;

        public ScoreDetailPage(ScoreDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        public ScoreDetailPage()
        {
            InitializeComponent();

            var data = new Score
            {
                Name = "Score name",
                ScoreTotal = 0
            };

            _viewModel = new ScoreDetailViewModel(data);
            BindingContext = _viewModel;
        }


        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoreEditPage(_viewModel));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoreDeletePage(_viewModel));
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}