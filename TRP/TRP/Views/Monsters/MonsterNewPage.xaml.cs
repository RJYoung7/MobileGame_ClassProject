using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonsterNewPage : ContentPage
    {
        // Data for this page
        public Monster Data { get; set; }

        // Constructor: creates blank monster that will get updated 
        public MonsterNewPage()
        {
            InitializeComponent();

            Data = new Monster
            {
                Name = "",
                Level = 1,
                Id = Guid.NewGuid().ToString(),
                Attribute = new AttributeBase(),
                MonsterType = MonsterTypeEnum.Unknown
            };

            BindingContext = this;

            // Set picker to what the Monster's type currently is
            MonsterTypePicker.SelectedItem = MonsterTypeList.GetMonsterTypeList[0].ToString();
        }

        // When save button is clicked, add attributes to this character, and broadcast add
        private async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddData", Data);
            await Navigation.PopAsync();
        }

        // When cancel button is clicked, remove this page from stack 
        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
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

        // The stepper function for Attack
        void Attack_OnStepperValueChanged(object sender, ValueChangedEventArgs e)
        {
            //var temp = AttackValue.GetValue + SpeedValue.GetValue + DefenseValue.GetValue;
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

        // Provides Monster information for picker
        private void MonsterTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var monsterType = MonsterTypePicker.SelectedItem.ToString();
            var monsterEnum = MonsterTypeList.ConvertStringToEnum(monsterType);
            pic.Source = Data.GetMonsterImage(monsterEnum);
        }
    }
}