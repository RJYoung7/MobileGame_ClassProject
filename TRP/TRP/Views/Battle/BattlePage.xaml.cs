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
using System.Diagnostics;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BattlePage : ContentPage
	{
        private BattleViewModel _viewModel;

        CharactersSelectPage _myCharactersSelectPage;

        GameOverPage _myGameOverPage;

        HtmlWebViewSource htmlSource = new HtmlWebViewSource();
        

        // Constructor: initialize battle page 
        public BattlePage (BattleViewModel viewmodel)
		{
			InitializeComponent ();
            BindingContext = _viewModel = BattleViewModel.Instance;

            _viewModel.BattleEngine.StartBattle(false);
            Debug.WriteLine("Battle Start" + " Characters: " + _viewModel.BattleEngine.CharacterList.Count);

            _viewModel.BattleEngine.StartRound();
            Debug.WriteLine("Round Start Monsters: " + _viewModel.BattleEngine.MonsterList.Count);

            // Add monsters if there weren't any, and only if there are penguins in party
            if (BattleViewModel.Instance.SelectedMonsters.Count == 0 && BattleViewModel.Instance.SelectedCharacters.Count >= 1)
            {
                foreach (var m in _viewModel.BattleEngine.MonsterList)
                {
                    BattleViewModel.Instance.SelectedMonsters.Add(m);
                    
                }
            }
            //BattleViewModel.Instance.SelectedMonsters.ElementAt(0).Alive;
            //var char1 =_viewModel.BattleEngine.CharacterList.ElementAt(0);
            //char1Name.Text = char1.Name;

        }

        private async void AttackButton_Clicked(object sender, EventArgs e)
        {
            await DisplayAlert(null, "Attacked", null, "Next");

        }

        public void ClearMessages() 
        {
            MessageText.Text = "";
            htmlSource.Html = _viewModel.BattleEngine.BattleMessage.GetHTMLBlankMessage();
            HtmlBox.Source = htmlSource;
        }

        public void AppendMessage(string message)
        {
            MessageText.Text = message + "\n" + MessageText.Text;
        }

        public void gameMessage()
        {
            var message = _viewModel.BattleEngine.TurnMessage;
            Debug.WriteLine("Message: " + message);

            AppendMessage(message);

            htmlSource.Html = _viewModel.BattleEngine.BattleMessage.GetHTMLFormattedTurnMessage();
            HtmlBox.Source = HtmlBox.Source = htmlSource;

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