using TRP.Models;

namespace TRP.ViewModels
{
    public class CharacterDetailViewModel : BaseViewModel
    {
        // Data for the view model 
        public Character Data { get; set; }

        // Constructor: takes in Character and binds data to view model
        public CharacterDetailViewModel(Character data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
