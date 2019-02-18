using TRP.Models;

namespace TRP.ViewModels
{
    public class MonsterDetailViewModel : BaseViewModel
    {
        // Data for the view model 
        public Monster Data { get; set; }

        // Constructor: takes in Monster and binds data to view model
        public MonsterDetailViewModel(Monster data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
