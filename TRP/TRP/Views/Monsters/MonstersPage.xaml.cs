using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;
using TRP.ViewModels;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MonstersPage : ContentPage
    {
        // ReSharper disable once NotAccessedField.Local
        private MonstersViewModel _viewModel;

        public MonstersPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = MonstersViewModel.Instance;
        }

    }
}