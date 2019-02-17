using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TRP.Models;
using TRP.ViewModels;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CharactersSelectPage : ContentPage
	{
        private CharactersViewModel _viewModel; // View model for this page

        // Constructor: creates new instance of this page, which initializes the xaml 
        public CharactersSelectPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = CharactersViewModel.Instance;
        }

        public IList<Character> party = new List<Character>();

        // Returns whether party is full 
        public bool PartyIsFull()
        {
            return party.Count() == GameGlobals.availCharactersSlots;
        }

        // Adds or removes the character to the party list from the switch toggle
        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            ViewCell cell = (sender as Switch).Parent.Parent as ViewCell;

            Character model = cell.BindingContext as Character;

            // Check if the select character is currently in party and if the party is full
            if (!party.Contains(model) && !PartyIsFull())
            {
                party.Add(model);

            }
            // Remove character from party and reset switch
            else
            {
                party.Remove(model);
                (sender as Switch).IsToggled = false;
            }

            partysize.Text = Convert.ToString(party.Count());
        }

        // Before the page appears, remove anything that was there prior, and load data to view model
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

        // Save the party of characters
        private void Save_Clicked(object sender, EventArgs e)
        {

        }

        // Cancel the the party selection and return to previous page
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}