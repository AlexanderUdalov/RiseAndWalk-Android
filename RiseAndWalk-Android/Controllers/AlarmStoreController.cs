using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RiseAndWalk_Android.Models;
using RiseAndWalk_Android.Services;

namespace RiseAndWalk_Android.Controllers
{
    public class AlarmStoreController
    {
        #region Singletone
        private static readonly Lazy<AlarmStoreController> _instanceHolder =
             new Lazy<AlarmStoreController>(() => new AlarmStoreController());

        public static AlarmStoreController Instance { get {
                if (_instanceHolder.Value.DataStore == null)
                    _instanceHolder.Value.DataStore = new MockDataStore();
                    return _instanceHolder.Value;
            }
        }
        #endregion

        public IDataStore<Alarm> DataStore;// => DataStore ?? new MockDataStore();
    }
}