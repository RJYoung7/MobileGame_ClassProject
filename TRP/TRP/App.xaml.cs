using System;

using TRP.Views;
using Xamarin.Forms;
using SQLite;

namespace TRP
{
    public partial class App : Application
    {
        static SQLiteAsyncConnection _database; // db for this app

        // Application constructor: loads database 
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            // Load The Mock Datastore by default
            TRP.Services.MasterDataStore.ToggleDataStore(Models.DataStoreEnum.Mock);
        }

        // SQLite initalization: where the database file can be found
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
