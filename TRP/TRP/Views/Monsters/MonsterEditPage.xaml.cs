using System;

using TRP.Models;
using TRP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MonsterEditPage : ContentPage
	{
	    // ReSharper disable once NotAccessedField.Local
	    private MonsterDetailViewModel _viewModel;

        public Monster Data { get; set; }

        public MonsterEditPage(MonsterDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Edit " + viewModel.Title;

            InitializeComponent();
            

            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
        }

	    private async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "EditData", Data);

            // removing the old ItemDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new items details page, with the new Item data on it
            await Navigation.PushAsync(new MonsterDetailPage(new MonsterDetailViewModel(Data)));

            // Last, remove this page
            Navigation.RemovePage(this);
        }

	    private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}