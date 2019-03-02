using TRP.ViewModels;
using TRP.Models;

namespace TRP.Services
{
    public static class MasterDataStore
    {
        // Holds which datastore to use.

        private static DataStoreEnum _dataStoreEnum = DataStoreEnum.Mock;

        // Returns which dtatstore to use
        public static DataStoreEnum GetDataStoreMockFlag()
        {
            return _dataStoreEnum;
        }

        // Switches the datastore values.
        // Loads the databases...
        public static void ToggleDataStore(DataStoreEnum dataStoreEnum)
        {
            switch (dataStoreEnum)
            {

                case DataStoreEnum.Mock:
                    _dataStoreEnum = DataStoreEnum.Mock;
                    ItemsViewModel.Instance.SetDataStore(DataStoreEnum.Mock);
                    CharactersViewModel.Instance.SetDataStore(DataStoreEnum.Mock);
                    MonstersViewModel.Instance.SetDataStore(DataStoreEnum.Mock);
                    ScoresViewModel.Instance.SetDataStore(DataStoreEnum.Mock);
                    break;

                case DataStoreEnum.SQL:
                default:
                    _dataStoreEnum = DataStoreEnum.SQL;
                    ItemsViewModel.Instance.SetDataStore(DataStoreEnum.SQL);
                    CharactersViewModel.Instance.SetDataStore(DataStoreEnum.SQL);
                    MonstersViewModel.Instance.SetDataStore(DataStoreEnum.SQL);
                    ScoresViewModel.Instance.SetDataStore(DataStoreEnum.SQL);
                    break;
            }

            // Load the Data
            ForceDataRestoreAll();
        }

        // Force all modes to load data...
        public static void ForceDataRestoreAll()
        {
            ItemsViewModel.Instance.ForceDataRefresh();
            MonstersViewModel.Instance.ForceDataRefresh();
            CharactersViewModel.Instance.ForceDataRefresh();
            ScoresViewModel.Instance.ForceDataRefresh();
        }
    }
}
