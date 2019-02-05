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
	    // ReSharper disable once NotAccessedField.Local
	    private CharacterDetailViewModel _viewModel;

        public Character Data { get; set; }

        public CharacterEditPage(CharacterDetailViewModel viewModel)
        {
            // Save off the item
            Data = viewModel.Data;
            viewModel.Title = "Edit " + viewModel.Title;

            InitializeComponent();
            
            // Set the data binding for the page
            BindingContext = _viewModel = viewModel;
            
            // Set the picker to the preexisting penguin type
            PenguinTypePicker.SelectedItem = Data.PType.ToString();
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

        public async void Save_Clicked(object sender, EventArgs e)
        {
            Data.ImageURI = Data.GetCharacterImage(Data.PType);
            Data.TypeBonus = Data.GetCharacterBonus(Data.PType);
            Data.BonusValue = Data.GetCharacterBonusValue(Data.PType);
            Data.BonusString = Data.GetBonusString(Data.PType);
            MessagingCenter.Send(this, "EditData", Data);

            // removing the old ItemDetails page, 2 up counting this page
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);

            // Add a new items details page, with the new Item data on it
            await Navigation.PushAsync(new CharacterDetailPage(new CharacterDetailViewModel(Data)));

            // Last, remove this page
            Navigation.RemovePage(this);
        }

	    private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // The stepper function for Attack
        void Attack_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            //var temp = AttackValue.GetValue + SpeedValue.GetValue + DefenseValue.GetValue;
            if (AttributeSum() > 10)
            {
                AttackValue.Text = String.Format("{0}", e.OldValue);
                attack.Value = e.OldValue;

            }
            else
            {
                AttackValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(10));

            }
        }

        // The stepper function for Defense
        void Defense_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (AttributeSum() > 10)
            {
                DefenseValue.Text = String.Format("{0}", e.OldValue);
                defense.Value = e.OldValue;

            }
            else
            {
                DefenseValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(10));

            }
        }

        // The stepper function for Speed
        void Speed_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (AttributeSum() > 10)
            {
                SpeedValue.Text = String.Format("{0}", e.OldValue);
                speed.Value = e.OldValue;

            }
            else
            {
                SpeedValue.Text = String.Format("{0}", e.NewValue);
                statPoints.Text = String.Format("{0}", AdjustStatPointAvail(10));

            }
        }
    }
}