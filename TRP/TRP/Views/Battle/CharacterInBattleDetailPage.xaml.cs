using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRP.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TRP.Views.Battle
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CharacterInBattleDetailPage : ContentPage
    {
        private CharacterDetailViewModel _viewModel;

		public CharacterInBattleDetailPage(CharacterDetailViewModel viewModel)
		{
			InitializeComponent();
            BindingContext = _viewModel = viewModel;

            var character = _viewModel.Data;
            if (character.Head != null)
            {
                HeadString.Text = ItemsViewModel.Instance.GetItem(character.Head).Name;
            }

            if (character.Feet != null)
            {
                FeetString.Text = ItemsViewModel.Instance.GetItem(character.Feet).Name;
            }

            if (character.Necklass != null)
            {
                NecklassString.Text = ItemsViewModel.Instance.GetItem(character.Necklass).Name;
            }

            if (character.Body != null)
            {
                BodyString.Text = ItemsViewModel.Instance.GetItem(character.Body).Name;
            }

            if (character.PrimaryHand != null)
            {
                PrimaryHandString.Text = ItemsViewModel.Instance.GetItem(character.PrimaryHand).Name;
            }

            if (character.OffHand != null)
            {
                OffHandString.Text = ItemsViewModel.Instance.GetItem(character.OffHand).Name;
            }

            if (character.RightFinger != null)
            {
                RightFingerString.Text = ItemsViewModel.Instance.GetItem(character.RightFinger).Name;
            }

            if (character.LeftFinger != null)
            {
                LeftFingerString.Text = ItemsViewModel.Instance.GetItem(character.LeftFinger).Name;
            }

            if (character.Bag != null)
            {
                BagString.Text = ItemsViewModel.Instance.GetItem(character.Bag).Name;
            }
        }
	}
}