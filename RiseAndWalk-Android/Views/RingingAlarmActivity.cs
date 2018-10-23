using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace RiseAndWalk_Android.Views
{
    [Activity(Label = "RingingAlarmActivity", Theme = "@style/AppThemeNoBar")]
    public class RingingAlarmActivity : BaseNfcReaderActivity
    {
        private Vibrator _vibrator;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_ringing_alarm);

            var args = Intent.GetBundleExtra("alarm");
            var description = args.GetString("description");
            var nfcTag = args.GetString("nfcTag");
            var ringedAlarmId = args.GetString("id");

            var button = FindViewById<Button>(Resource.Id.ringing_alarm_button_close);
            var descriptionView = FindViewById<TextView>(Resource.Id.ringing_alarm_description);
            var nfcHelpText = FindViewById<TextView>(Resource.Id.ringing_alarm_needs_nfc_tag);

            descriptionView.Text = description;

            button.Visibility = string.IsNullOrEmpty(nfcTag)? ViewStates.Visible: ViewStates.Invisible;
            button.Click += delegate { FinishWithAlarmId(ringedAlarmId); };

            nfcHelpText.Visibility = !string.IsNullOrEmpty(nfcTag) ? ViewStates.Visible : ViewStates.Invisible;

            _vibrator = Vibrator.FromContext(this);
            _vibrator.Vibrate(VibrationEffect.CreateWaveform(new long[]{500, 1000}, 0));

            OnNfcTagDetected += (hash) =>
            {
                if (hash != nfcTag) return;
                FinishWithAlarmId(ringedAlarmId);
            };
        }

        private void FinishWithAlarmId(string ringedAlarmId)
        {
            _vibrator.Cancel();
            var resultIntent = new Intent(this, typeof(MainActivity));
            resultIntent.SetFlags(ActivityFlags.ClearTask);
            resultIntent.PutExtra("ringingAlarmId", ringedAlarmId);

            StartActivity(resultIntent);
            Finish();
        }

        //It's necessary to cancel finishing activity by 'back' button click
        public override void OnBackPressed() { }
    }
}