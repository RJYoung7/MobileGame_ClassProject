using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoreNewPage : ContentPage
    {
        public Score Data { get; set; }

        public ScoreNewPage()
        {
            InitializeComponent();

            Data = new Score
            {
                Name = "Score name",
                ScoreTotal = 0,
                Id = Guid.NewGuid().ToString()
            };

            BindingContext = this;
        }

        private async void Save_Clicked(object sender, EventArgs e)
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