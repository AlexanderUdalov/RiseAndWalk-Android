using System;
using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Widget;

namespace RiseAndWalk_Android.Views
{
    public abstract class BaseNfcReaderActivity : Activity
    {
        private bool _inWriteMode;
        private NfcAdapter _nfcAdapter;

        public event Action<string> OnNfcTagDetected;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
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
                    //TODO: Do something
                });
                alert.Show();
            }
            else
                _nfcAdapter.EnableForegroundDispatch(this, pendingIntent, filters, null);
        }

        protected override void OnNewIntent(Intent intent)
        {
            if (!_inWriteMode) return;

            _inWriteMode = false;
            var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                var hash = sha.ComputeHash(tag.GetId());
                var hashString = BitConverter.ToString(hash).Replace("-", "");

                OnNfcTagDetected?.Invoke(hashString);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            EnableWriteMode();
        }

        protected override void OnPause()
        {
            _nfcAdapter?.DisableForegroundDispatch(this);
            base.OnPause();
        }
    }
}