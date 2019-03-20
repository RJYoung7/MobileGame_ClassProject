using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
	{
        // Constructor: stands the main page of the application up
		public MainPage ()
		{
			InitializeComponent ();
		}

        // Resets navigation stack when moving to this page
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            ((NavigationPage)CurrentPage).PopToRootAsync();

        }
    }
}