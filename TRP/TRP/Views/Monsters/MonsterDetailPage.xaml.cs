using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;
using TRP.ViewModels;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MonsterDetailPage : ContentPage
    {
        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private MonsterDetailViewModel _viewModel;

        public MonsterDetailPage(MonsterDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = _viewModel = viewModel;
        }

        public MonsterDetailPage()
        {
            InitializeComponent();

            var data = new Monster
            {
                Name = "Item 1",
                Description = "This is an item description."
            };

            _viewModel = new MonsterDetailViewModel(data);
            BindingContext = _viewModel;
        }

        private async void Edit_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonsterEditPage(_viewModel));
        }

        private async void Delete_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MonsterDeletePage(_viewModel));
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}