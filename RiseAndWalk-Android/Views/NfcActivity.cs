using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RiseAndWalk_Android.Views
{

    [Activity(Label = "NfcActivity")]
    class NfcActivity : Activity
    {

        private bool _inWriteMode;
        private NfcAdapter _nfcAdapter;
        private TextView _textView;

        public event Action<string> OnNfcTagDetected;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_read_nfc);

            _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);
            //EnableWriteMode();

            _textView = FindViewById<TextView>(Resource.Id.text_nfc_tag);

            OnNfcTagDetected += (id) =>
            {
                _textView.Text = id;
            };
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
                alert.SetButton("OK", delegate {
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

                StringBuilder hex = new StringBuilder();
                foreach (byte b in tag.GetId())
                    hex.AppendFormat("{0:x2}", b);

                OnNfcTagDetected?.Invoke(hex.ToString());
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