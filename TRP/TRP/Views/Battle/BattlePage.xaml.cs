using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TRP.ViewModels;
using TRP.Models;
using System.Collections.ObjectModel;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BattlePage : ContentPage
	{
        private BattleViewModel _viewModel;


        public BattlePage (BattleViewModel viewmodel)
		{
			InitializeComponent ();
            BindingContext = _viewModel = BattleViewModel.Instance;

            _viewModel.BattleEngine.StartRound();

            foreach (var m in _viewModel.BattleEngine.MonsterList)
            {
                BattleViewModel.Instance.SelectedMonsters.Add(m);
                
            }
            //BattleViewModel.Instance.SelectedMonsters.ElementAt(0).Alive;
            //var char1 =_viewModel.BattleEngine.CharacterList.ElementAt(0);
            //char1Name.Text = char1.Name;

        }

        private async void AttackButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert(null, "Attacked", null, "Next");

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = null;

            if (ToolbarItems.Count > 0)
            {
                ToolbarItems.RemoveAt(0);
            }

            InitializeComponent();

            if (_viewModel.BattleEngine.CharacterList.Count == 0 || _viewModel.BattleEngine.MonsterList.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }

            BindingContext = _viewModel;
        }
    }
}