using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;
using TRP.ViewModels;
// ReSharper disable FieldCanBeMadeReadOnly.Local
// ReSharper disable RedundantExtendsListEntry

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterDetailPage : ContentPage
    {
        private CharacterDetailViewModel _viewModel; // View model for this page

        // Constructor: overload with view model 
        public CharacterDetailPage(CharacterDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        //When edit button is clicked, create edit page and add onto stack
        public async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharacterEditPage(_viewModel));
        }

        // When delete button is clicked, create delete page and add onto stack
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CharacterDeletePage(_viewModel));
        }

        // When cancel button is clicked, remove this page from stack
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

    }
}