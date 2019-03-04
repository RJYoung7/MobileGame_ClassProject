 
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
	{
		public MainPage ()
		{
			InitializeComponent ();
		}

        protected override void OnCurrentPageChanged()
        {

            base.OnCurrentPageChanged();

            ((NavigationPage)CurrentPage).PopToRootAsync();

        }
    }
}