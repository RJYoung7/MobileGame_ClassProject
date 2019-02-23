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
        private ScoreDetailViewModel _viewModel; // View model for this page

        // Constructor: overload with view model
        public ScoreDetailPage(ScoreDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        // Constructor: creates instance of page, which initializes the xaml 
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

        // When edit button is clicked, create edit page and add onto stack
        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoreEditPage(_viewModel));
        }

        // When delete button is clicked, create delete page and add onto stack
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScoreDeletePage(_viewModel));
        }

        // When cancel button is clicked, remove this page from stack
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}