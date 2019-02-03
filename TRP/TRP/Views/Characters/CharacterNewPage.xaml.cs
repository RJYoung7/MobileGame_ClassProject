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
            MessagingCenter.Send(this, "AddData", Data);
            await Navigation.PopAsync();
        }

        private async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}