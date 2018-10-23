using RiseAndWalk_Android.Models;
using System;
using System.Collections.Generic;

namespace RiseAndWalk_Android.Controllers
{
    public class AlarmStoreController
    {
        #region Singletone

        private static readonly Lazy<AlarmStoreController> _instanceHolder =
             new Lazy<AlarmStoreController>(() => new AlarmStoreController());

        public static AlarmStoreController Instance => _instanceHolder.Value;

        #endregion Singletone

        public Action OnDataStoreChanged;
        private readonly SQLiteDatabase<Alarm> _db = new SQLiteDatabase<Alarm>("UserAlarms");

        public IEnumerable<Alarm> GetAlarms() => _db.Get();

        public void AddAlarm(Alarm alarm)
        {
            foreach (var alarm1 in GetAlarms())
            {
                _db.Delete(alarm1);
            }
            _db.Add(alarm);
            OnDataStoreChanged?.Invoke();
        }

        public void DeleteAlarm(Alarm alarm)
        {
            _db.Delete(alarm);
            OnDataStoreChanged?.Invoke();
        }

        public void UpdateAlarm(Alarm alarm)
        {
            _db.Update(alarm);
            OnDataStoreChanged?.Invoke();
        }
        
        //TODO: доделать
        public void DeleteOrRepeatRingingAlarm(string ringingAlarmId)
        {
            var guid = Guid.Parse(ringingAlarmId);
            if (_db.Get(guid) == null) return;


            _db.Delete(guid);
        }
    }
}