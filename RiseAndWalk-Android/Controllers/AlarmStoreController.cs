using RiseAndWalk_Android.Models;
using RiseAndWalk_Android.Services;
using System;

namespace RiseAndWalk_Android.Controllers
{
    public class AlarmStoreController
    {
        #region Singletone

        private static readonly Lazy<AlarmStoreController> _instanceHolder =
             new Lazy<AlarmStoreController>(() => new AlarmStoreController());

        public static AlarmStoreController Instance
        {
            get
            {
                if (_instanceHolder.Value.DataStore == null)
                    _instanceHolder.Value.DataStore = new MockDataStore();
                return _instanceHolder.Value;
            }
        }

        #endregion Singletone

        public IDataStore<Alarm> DataStore;// => DataStore ?? new MockDataStore();
    }
}