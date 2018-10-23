using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using RiseAndWalk_Android.Models;
using System;
using System.Linq;
using Android.Widget;

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

            var args = new Bundle();
            args.PutString("description", alarm.Description);
            args.PutString("nfcTag", alarm.NfcTagHash);
            args.PutString("id", alarm.Id.ToString());

            alarmIntent.PutExtra("alarm", args);

            var pending = PendingIntent.GetBroadcast(context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();

            var time = CalculateAlarmTime(alarm);
            var remainedTime = time - DateTime.Now;

            Toast.MakeText(context, 
                $"Будильник сработает через {remainedTime.Days} дней {remainedTime.Hours} часов {remainedTime.Minutes} минут", 
                ToastLength.Long).Show();

            alarmManager.Set(AlarmType.RtcWakeup, ConvertToUnixTime(time), pending);
        }

        private DateTime CalculateAlarmTime(Alarm alarm)
        {
            if (alarm.DaysOfWeek == null ||
                alarm.DaysOfWeek.Length == 0)
                return GetOnetimeAlarmNextTime(alarm.Time);

            if (alarm.DaysOfWeek.Contains(DateTime.Now.DayOfWeek))
                if (DateTime.Now.TimeOfDay < alarm.Time)
                    return GetDateTime(DateTime.Now, alarm.Time);

            var nextAlarmDay = alarm.DaysOfWeek.Select(day => GetNextWeekday(DateTime.Now, day)).Min();
            return GetDateTime(nextAlarmDay, alarm.Time);
        }

        private DateTime GetDateTime(DateTime day, TimeSpan time)
            => new DateTime(day.Year, day.Month, day.Day, time.Hours, time.Minutes, 0);

        private DateTime GetOnetimeAlarmNextTime(TimeSpan time)
            => GetDateTime(time > DateTime.Now.TimeOfDay ? DateTime.Now : DateTime.Now.AddDays(1), time);

        private DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            var daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysToAdd);
        }

        private static long ConvertToUnixTime(DateTime time)
        {
            return (long)time.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }

        public void DisableAlarm(Context context, Alarm alarm)
        {
            var alarmIntent = new Intent(context, typeof(AlarmReceiver));
            var pending = PendingIntent.GetBroadcast(context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);

            var alarmManager = context.GetSystemService(Context.AlarmService).JavaCast<AlarmManager>();
            alarmManager.Cancel(pending);
        }
    }
}