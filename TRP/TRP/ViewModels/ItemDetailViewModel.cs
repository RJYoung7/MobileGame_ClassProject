using TRP.Models;

namespace TRP.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        // Data for the view model
        public Item Data { get; set; }

        // Constructor: takes in an item and binds data to view model
        public ItemDetailViewModel(Item data = null)
        {
            Title = data?.Name;
            Data = data;
        }
    }
}
