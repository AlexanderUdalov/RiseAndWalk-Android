using Android.Content;
using RiseAndWalk_Android.Views;

namespace RiseAndWalk_Android.Controllers
{
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var resultIntent = new Intent(context, typeof(RingingAlarmActivity));
            resultIntent.SetFlags(ActivityFlags.NewTask);

            context.StartActivity(resultIntent);
        }
    }
}