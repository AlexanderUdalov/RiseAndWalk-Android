using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Widget;
using System;

namespace RiseAndWalk_Android.Views
{
    [Activity(Label = "NfcActivity")]
    internal class NfcActivity : Activity
    {
        private bool _inWriteMode;
        private NfcAdapter _nfcAdapter;
        private TextView _textView;

        private string _nfcTagHash;
        private string _cachedId;

        public event Action<string> OnNfcTagDetected;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_read_nfc);

            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

            _textView = FindViewById<TextView>(Resource.Id.text_nfc_tag);

            var acceptTagButton = FindViewById<TextView>(Resource.Id.button_accept_nfc_tag);
            acceptTagButton.Click += delegate { OnAcceptTag(); };

            OnNfcTagDetected += (id) =>
            {
                if (id.Equals(_cachedId))
                {
                    Vibrator vibrator = (Vibrator)GetSystemService(VibratorService);
                    vibrator.Vibrate(VibrationEffect.CreateOneShot(100, VibrationEffect.DefaultAmplitude));
                }
                _cachedId = id;
                _nfcTagHash = id;
                _textView.Text = id;
            };
        }

        private void OnAcceptTag()
        {
            Intent intent = new Intent();
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

        private void EnableWriteMode()
        {
            _inWriteMode = true;

            var tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
            var filters = new[] { tagDetected };

            var intent = new Intent(this, GetType()).AddFlags(ActivityFlags.SingleTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

            if (_nfcAdapter == null)
            {
                var alert = new AlertDialog.Builder(this).Create();
                alert.SetMessage("NFC is not supported on this device.");
                alert.SetTitle("NFC Unavailable");
                alert.SetButton("OK", delegate
                {
                    _textView.Text = "NFC is not supported on this device.";
                });
                alert.Show();
            }
            else
                _nfcAdapter.EnableForegroundDispatch(this, pendingIntent, filters, null);
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (_inWriteMode)
            {
                _inWriteMode = false;
                var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;

                using (var sha = new System.Security.Cryptography.SHA256Managed())
                {
                    byte[] hash = sha.ComputeHash(tag.GetId());
                    var hashString = BitConverter.ToString(hash).Replace("-", "");

                    OnNfcTagDetected?.Invoke(hashString);
                }
                return;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            EnableWriteMode();
        }

        protected override void OnPause()
        {
            if (_nfcAdapter != null)
                _nfcAdapter.DisableForegroundDispatch(this);

            base.OnPause();
        }
    }
}