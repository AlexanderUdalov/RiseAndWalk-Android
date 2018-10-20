using Android.App;
using Android.Content;
using Android.Runtime;
using RiseAndWalk_Android.Models;
using System;

namespace RiseAndWalk_Android.Controllers
{
    public class AlarmServiceController
    {
        #region Singletone

        private static readonly Lazy<AlarmServiceController> _instanceHolder =
              new Lazy<AlarmServiceController>(() => new AlarmServiceController());

        public static AlarmServiceController Instance => _instanceHolder.Value;

        #endregion Singletone

        public void EnableAlarm(Context context, Alarm alarm)
        {
            var alarmIntent = new Intent(context, typeof(AlarmReceiver));

            var pending = PendingIntent.GetBroadcast(context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();

            alarmManager.Set(AlarmType.RtcWakeup, ConvertToUnixTime(alarm.Time), pending);
        }

        public void DisableAlarm(Alarm alarm)
        {
            throw new NotImplementedException();
        }

        private static long ConvertToUnixTime(DateTime time)
        {
            return (long)time.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }
    }
}