using TRP.Models;

namespace TRP.ViewModels
{
    public class ScoreDetailViewModel : BaseViewModel
    {
        // Data for the view model 
        public Score Data { get; set; }

        // Constructor: takes in Score and binds data to view model
        public ScoreDetailViewModel(Score data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
