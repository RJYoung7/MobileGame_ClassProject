using System;
using TRP.Controllers;
using TRP.Models;
using TRP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CharacterEditPage : ContentPage
	{
	    private CharacterDetailViewModel _viewModel; // View model for this page

        // Data for this page
        public Character Data { get; set; }

        // Constructor: binds data for view and saves bound data
        public CharacterEditPage(CharacterDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Edit " + viewModel.Title;

            InitializeComponent();
            
            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
            
            // Set the picker to the preexisting penguin type
            PenguinTypePicker.SelectedItem = Data.PenguinType.ToString();
        }

        // Returns the sum of all attributes
        public int AttributeSum()
        {
            return Data.Attribute.Attack + Data.Attribute.Defense + Data.Attribute.Speed;
        }

        // Returns the number of stat points available
        public int AdjustStatPointAvail(int statTotalPoints)
        {
            return statTotalPoints - (AttributeSum());
        }

        // When save button is clicked, add attributes to this character, and broadcast edit
        public async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "EditData", Data);

            // removing the old ItemDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new items details page, with the new Item data on it
            await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(Data)));

            await Navigation.PopAsync();

            // Last, remove this page
            Navigation.RemovePage(this);

        }

        // When cancel button is clicked, remove this page from stack
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // The stepper function for Attack
        void Attack_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (AttributeSum() > GameGlobals.availStatPoints)
            {
                AttackValue.Text = String.Format("{0}", e.OldValue);
                attack.Value = e.OldValue;
            }
            else
            {
                AttackValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(GameGlobals.availStatPoints));
            }
        }

        // The stepper function for Defense
        void Defense_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (AttributeSum() > GameGlobals.availStatPoints)
            {
                DefenseValue.Text = String.Format("{0}", e.OldValue);
                defense.Value = e.OldValue;
            }
            else
            {
                DefenseValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(GameGlobals.availStatPoints));
            }
        }

        // The stepper function for Speed
        void Speed_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (AttributeSum() > GameGlobals.availStatPoints)
            {
                SpeedValue.Text = String.Format("{0}", e.OldValue);
                speed.Value = e.OldValue;
            }
            else
            {
                SpeedValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(GameGlobals.availStatPoints));
            }
        }

        // Provides Penguin infomration for picker
        private void PenguinTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var penguinType = PenguinTypePicker.SelectedItem.ToString();
            var penguinEnum = PenguinTypeList.ConvertStringToEnum(penguinType);
            pic.Source = Data.GetCharacterImage(penguinEnum);
        }
    }
}