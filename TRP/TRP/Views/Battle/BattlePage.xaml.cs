using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

using TRP.ViewModels;
using TRP.Models;
using TRP.GameEngine;

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

            Debug.WriteLine("Round Start Monsters: " + _viewModel.BattleEngine.MonsterList.Count);

            // round number at top of page
            numRounds.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.RoundCount);
            
        }

        // When next turn is clicked, start next turn, or end game by checking game state
        private async void NextTurnButton_Clicked(object sender, EventArgs e)
        {
            ClearMessages();
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
                RoundStartMessage();
                Navigation.RemovePage(this);
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
                Navigation.RemovePage(this);
                return;
            }

            // Output the Game Board
            _viewModel.LoadDataCommand.Execute(null);

            //InitializeComponent();
            numRounds.Text = Convert.ToString(_viewModel.BattleEngine.BattleScore.RoundCount);

            RoundStartMessage();
            // Output The Message that happened.
            gameMessage();
        }

        // TODO
        private async void AttackButton_Clicked(object sender, EventArgs e)
        {
            //should only be pressed during character's turn 
        }

        // Use consumable item
        private async void UseItemButton_Clicked(object sender, EventArgs e)
        {

            if(_viewModel.BattleEngine.BattleScore.TurnCount < 1)
            {
                _viewModel.BattleEngine.BattleMessage.TurnMessageSpecial = "All Characters full health";
                gameMessage();
            }
            else
            {
               
                _viewModel.BattleEngine.ConsumeItem(_viewModel.BattleEngine.CurrentCharacter);

                gameMessage();
            }

        }
        
        // Clears messages in html box 
        public void ClearMessages() 
        {
            MessageText.Text = "";
            //htmlSource.Html = _viewModel.BattleEngine.BattleMessage.GetHTMLBlankMessage();
            //HtmlBox.Source = htmlSource;
        }

        // Adds message to be shown in html box 
        public void AppendMessage(string message)
        {
            MessageText.Text = message + "\n" + MessageText.Text;
        }

        // TODO: fix messages above box
        // Displays the messages in the game 
        public void gameMessage()
        {
            var message = _viewModel.BattleEngine.BattleMessage.GetTurnMessageString();
            Debug.WriteLine("Message: " + message);

            MessageText.Text = message;

            //htmlSource.Html = _viewModel.BattleEngine.BattleMessage.GetHTMLFormattedTurnMessage();
            //HtmlBox.Source = HtmlBox.Source = htmlSource;
        }

        //
        public void RoundStartMessage()
        {

            var message = _viewModel.BattleEngine.BattleMessage.TimeWarpMessage;

            MessageText.Text = message;

            //htmlSource.Html = _viewModel.BattleEngine.BattleMessage.GetHTMLFormattedRoundMessage();
            //HtmlBox.Source = HtmlBox.Source = htmlSource;
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
            MessageText.Text = _viewModel.BattleEngine.BattleMessage.TimeWarpMessage;
        }
    }
}