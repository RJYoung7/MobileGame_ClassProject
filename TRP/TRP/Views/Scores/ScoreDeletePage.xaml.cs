using TRP.Models;
using TRP.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScoreDeletePage : ContentPage
	{
	    private ScoreDetailViewModel _viewModel; // View model for this page

        public Score Data { get; set; } // Data for this page

        // Constructor: creates instance of this page, which initializes the xaml
        public ScoreDeletePage (ScoreDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Delete " + viewModel.Title;

            InitializeComponent();

            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
        }

        // When delete button is clicked, broadcast delete and pop this page off stack
        private async void Delete_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "DeleteData", Data);

            // Remove Item Details Page manualy
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            await Navigation.PopAsync();
        }

        // When cancel button is clicked, remove this page from stack
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}