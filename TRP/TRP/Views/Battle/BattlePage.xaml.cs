using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

using TRP.ViewModels;
using TRP.Models;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BattlePage : ContentPage
	{
        private BattleViewModel _viewModel; // view model for the page 

        HtmlWebViewSource htmlSource = new HtmlWebViewSource(); // window for messages
        
        // Constructor: initialize battle page 
        public BattlePage (BattleViewModel viewmodel)
		{
			InitializeComponent ();
            BindingContext = _viewModel = BattleViewModel.Instance;
            _viewModel.ClearCharacterLists();

            _viewModel.BattleEngine.StartBattle(false);
            Debug.WriteLine("Battle Start" + " Characters: " + _viewModel.BattleEngine.CharacterList.Count);

            _viewModel.BattleEngine.NewRound();
            Debug.WriteLine("Round Start Monsters: " + _viewModel.BattleEngine.MonsterList.Count);

            // Add monsters if there weren't any, and only if there are penguins in party
            if (BattleViewModel.Instance.SelectedMonsters.Count == 0 && BattleViewModel.Instance.SelectedCharacters.Count >= 1)
            {
                foreach (var m in _viewModel.BattleEngine.MonsterList)
                {
                    BattleViewModel.Instance.SelectedMonsters.Add(m);
                    
                }
            }

            // round number at top of page
            numRounds.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.RoundCount);
        }

        // When next turn is clicked, start next turn, or end game by checking game state
        private async void NextTurnButton_Clicked(object sender, EventArgs e)
        {
            _viewModel.RoundNextTurn();

            // Hold the current state
            var CurrentRoundState = _viewModel.BattleEngine.RoundStateEnum;
            Debug.WriteLine("Round: " + CurrentRoundState);

            // If the round is over start a new one...
            if (CurrentRoundState == RoundEnum.NewRound)
            {
                //_viewModel.NewRound();
                MessagingCenter.Send(this, "NewRound");
                await Navigation.PushAsync(new RoundEndPage(_viewModel));
                Debug.WriteLine("New Round: " + _viewModel.BattleEngine.BattleScore.RoundCount);
            }

            // Check for Game Over
            if (CurrentRoundState == RoundEnum.GameOver)
            {
                MessagingCenter.Send(this, "EndBattle", RoundEnum.GameOver);

                Debug.WriteLine("End Battle");

                // Output Formatted Results 
                var myResult = _viewModel.BattleEngine.GetResultsOutput();
                Debug.Write(myResult);

                // Let the user know the game is over
                ClearMessages();    // Clear message
                AppendMessage("Game Over\n"); // Show Game Over
                await Navigation.PushAsync(new GameOverPage());
                return;
            }

            // Output the Game Board
            _viewModel.LoadDataCommand.Execute(null);

            //InitializeComponent();
            numRounds.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.RoundCount);

            // Output The Message that happened.
            gameMessage();
        }

        // TODO
        private async void AttackButton_Clicked(object sender, EventArgs e)
        {
            //should only be pressed during character's turn 
        }
        
        // Clears messages in html box 
        public void ClearMessages() 
        {
            MessageText.Text = "";
            htmlSource.Html = _viewModel.BattleEngine.BattleMessage.GetHTMLBlankMessage();
            HtmlBox.Source = htmlSource;
        }

        // Adds message to be shown in html box 
        public void AppendMessage(string message)
        {
            MessageText.Text = message + "\n" + MessageText.Text;
        }

        // Writes message in html box 
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
            _viewModel.ClearCharacterLists();

            if (_viewModel.BattleEngine.CharacterList.Count == 0 || _viewModel.BattleEngine.MonsterList.Count == 0)
            {
                _viewModel.LoadDataCommand.Execute(null);
            }
            else if (_viewModel.NeedsRefresh())
            {
                _viewModel.LoadDataCommand.Execute(null);
            }

            BindingContext = _viewModel;
            numRounds.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.RoundCount);
        }
    }
}