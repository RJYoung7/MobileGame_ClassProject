using System;

using TRP.Views;
using Xamarin.Forms;
using SQLite;

namespace TRP
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            // Load The Mock Datastore by default
            TRP.Services.MasterDataStore.ToggleDataStore(Models.DataStoreEnum.Mock);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }


        static SQLiteAsyncConnection _database;

        public static SQLiteAsyncConnection Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new SQLiteAsyncConnection(DependencyService.Get<IFileHelper>().GetLocalFilePath("TRPDatabase.db3"));
                }
                return _database;
            }
        }

    }
}
