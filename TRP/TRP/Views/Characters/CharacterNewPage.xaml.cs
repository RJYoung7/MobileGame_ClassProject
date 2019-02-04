using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterNewPage : ContentPage
    {
        public Character Data { get; set; }

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

        public CharacterNewPage()
        {
            InitializeComponent();

            Data = new Character
            {
                Name = "",
                //Description = "This is a Character description.",
                //Level = 1,
                //Id = Guid.NewGuid().ToString(),
                Attribute = new AttributeBase(),
                PType = PenguinType.Unknown
            };

            BindingContext = this;
            
            PenguinTypePicker.SelectedItem = Data.PType.ToString();
 
        }

        public async void Save_Clicked(object sender, EventArgs e)
        {
            Data.ImageURI = Data.GetCharacterImage(Data.PType);
            Data.TypeBonus = Data.GetCharacterBonus(Data.PType);
            Data.BonusValue = Data.GetCharacterBonusValue(Data.PType);
            MessagingCenter.Send(this, "AddData", Data);
            await Navigation.PopAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        // The stepper function for Attack
        void Attack_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            //var temp = AttackValue.GetValue + SpeedValue.GetValue + DefenseValue.GetValue;
            if(AttributeSum() > 10)
            {
                AttackValue.Text = String.Format("{0}", e.OldValue);
                attack.Value = e.OldValue;
                  
            } else
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