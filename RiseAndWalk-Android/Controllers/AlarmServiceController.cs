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
using RiseAndWalk_Android.Views;

namespace RiseAndWalk_Android.Controllers
{
    // Не работыет XD
    public class AlarmServiceController
    {
        #region Singletone
        private static readonly Lazy<AlarmServiceController> _instanceHolder =
              new Lazy<AlarmServiceController>(() => new AlarmServiceController());

        public static AlarmServiceController Instance => _instanceHolder.Value;
        #endregion
        
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

        private long ConvertToUnixTime(DateTime time)
        {
            return (long)time.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }

        [BroadcastReceiver]
        private class AlarmReceiver : BroadcastReceiver
        {
            //TODO: изменить на активити будильника
            public override void OnReceive(Context context, Intent intent)
            {
                var message = intent.GetStringExtra("message");
                var title = intent.GetStringExtra("title");

                var resultIntent = new Intent(context, typeof(HelloActivity));
                resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);

                var pending = PendingIntent.GetActivity(context, 0,
                    resultIntent,
                    PendingIntentFlags.CancelCurrent);

                var builder =
                    new Notification.Builder(context)
                        .SetContentTitle(title)
                        .SetContentText(message)
                        .SetSmallIcon(Resource.Drawable.abc_btn_radio_material)
                        .SetDefaults(NotificationDefaults.All);

                builder.SetContentIntent(pending);

                var notification = builder.Build();

                var manager = NotificationManager.FromContext(context);
                manager.Notify(1337, notification);
            }
        }
    }
}