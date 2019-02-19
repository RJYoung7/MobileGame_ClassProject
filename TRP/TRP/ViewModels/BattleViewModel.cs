using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using TRP.Models;
using TRP.Views;
using System.Linq;
using TRP.GameEngine;
using TRP.Views.Battle;
using System.Collections.Generic;

namespace TRP.ViewModels
{
    public class BattleViewModel : BaseViewModel
    {
        // Make this a singleton so it only exist one time because holds all the data records in memory
        private static BattleViewModel _instance;

        // Constructor: returns instance if instantiated, otherwise creates instance if it's null 
        public static BattleViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BattleViewModel();
                }
                return _instance;
            }
        }

        // Battle Engine
        private BattleEngine battleEngine;

        // Constructor: loads data and listens for broadcast from views
        public BattleViewModel()
        {
            Title = "Battle Begin";
            battleEngine = new BattleEngine();

            // For adding Characters to party
            MessagingCenter.Subscribe<CharactersSelectPage, IList<Character>>(this, "AddData", (obj, data) =>
            {
                battleEngine.CharacterList = data.ToList<Character>();
            });
        }
    }
}