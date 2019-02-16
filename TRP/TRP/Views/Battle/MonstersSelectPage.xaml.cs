using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.ViewModels;
using TRP.Models;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MonstersSelectPage : ContentPage
	{
        private MonstersViewModel _viewModel; // View model for this page

        // Constructor: creates new instance of this page, which initializes the xaml 
        public MonstersSelectPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = MonstersViewModel.Instance;
        }

        public IList<Monster> party = new List<Monster>();

        // Returns whether party is full 
        public bool PartyIsFull()
        {
            return party.Count() == GameGlobals.availMonstersSlots;
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            ViewCell cell = (sender as Switch).Parent.Parent as ViewCell;

            Monster model = cell.BindingContext as Monster;

            if (!party.Contains(model) && !PartyIsFull())
            {
                party.Add(model);

            }
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

        private void Save_Clicked(object sender, EventArgs e)
        {

        }

        private void Cancel_Clicked(object sender, EventArgs e)
        {

        }

    }
}