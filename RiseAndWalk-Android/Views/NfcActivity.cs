using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;

namespace RiseAndWalk_Android.Views
{
    [Activity(Label = "NfcActivity")]
    internal class NfcActivity : BaseNfcReaderActivity
    {
        private TextView _textView;

        private string _nfcTagHash;
        private string _cachedId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_read_nfc);

            _textView = FindViewById<TextView>(Resource.Id.text_nfc_tag);

            var acceptTagButton = FindViewById<TextView>(Resource.Id.button_accept_nfc_tag);
            acceptTagButton.Click += delegate { OnAcceptTag(); };

            OnNfcTagDetected += (id) =>
            {
                if (id.Equals(_cachedId))
                {
                    var vibrator = (Vibrator)GetSystemService(VibratorService);
                    vibrator.Vibrate(VibrationEffect.CreateOneShot(100, VibrationEffect.DefaultAmplitude));
                }
                _cachedId = id;
                _nfcTagHash = id;
                _textView.Text = id;
            };
        }

        private void OnAcceptTag()
        {
            var intent = new Intent();
            if (string.IsNullOrEmpty(_nfcTagHash))
            {
                SetResult(Result.Canceled, intent);
                Finish();
            }
            else
            {
                intent.PutExtra("nfcTagHash", _nfcTagHash);
                SetResult(Result.Ok, intent);
                Finish();
            }
        }
    }
}