using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TRP.Models;

namespace TRP.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewMainPageMaster : ContentPage
    {
        NewMainPage RootPage { get => Application.Current.MainPage as NewMainPage; }
        List<HomeMenuItem> menuItems;

        public NewMainPageMaster()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Home, Title="Home"},
                new HomeMenuItem {Id = MenuItemType.About, Title="About" },
                new HomeMenuItem {Id = MenuItemType.Monsters, Title="Monsters"},
                new HomeMenuItem {Id = MenuItemType.Characters, Title="Characters"},
                new HomeMenuItem {Id = MenuItemType.Items, Title="Items"},
                new HomeMenuItem {Id = MenuItemType.History, Title = "History"}
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.SelectedItem = menuItems[0];
            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;

                var id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }

        //class NewMainPageMasterViewModel : INotifyPropertyChanged
        //{
        //    public ObservableCollection<NewMainPageMenuItem> MenuItems { get; set; }
            
        //    public NewMainPageMasterViewModel()
        //    {
        //        MenuItems = new ObservableCollection<NewMainPageMenuItem>(new[]
        //        {
        //            new NewMainPageMenuItem { Id = 0, Title = "Page 1" },
        //            new NewMainPageMenuItem { Id = 1, Title = "Page 2" },
        //            new NewMainPageMenuItem { Id = 2, Title = "Page 3" },
        //            new NewMainPageMenuItem { Id = 3, Title = "Page 4" },
        //            new NewMainPageMenuItem { Id = 4, Title = "Page 5" },
        //        });
        //    }
            
        //    #region INotifyPropertyChanged Implementation
        //    public event PropertyChangedEventHandler PropertyChanged;
        //    void OnPropertyChanged([CallerMemberName] string propertyName = "")
        //    {
        //        if (PropertyChanged == null)
        //            return;

        //        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //    #endregion
        //}
    }
}