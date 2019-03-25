using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;
using TRP.ViewModels;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonsterDetailPage : ContentPage
    {
        private MonsterDetailViewModel _viewModel; // View model for this page

        // Constructor: overload with view model
        public MonsterDetailPage(MonsterDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        // Constructor: creates instance of page, which initializes the xaml 
        public MonsterDetailPage()
        {
            InitializeComponent();

            var data = new Monster();

            _viewModel = new MonsterDetailViewModel(data);
            BindingContext = _viewModel;
        }

        // When edit button is clicked, create edit page and add onto stack
        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonsterEditPage(_viewModel));
        }

        // When delete button is clicked, create delete page and add onto stack
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonsterDeletePage(_viewModel));
        }

        // When cancel button is clicked, remove this page from stack
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}