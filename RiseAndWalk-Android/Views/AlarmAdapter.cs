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
using Java.Lang;
using RiseAndWalk_Android.Models;

namespace RiseAndWalk_Android.Views
{
    class AlarmAdapter : BaseAdapter<Alarm>
    {
        public List<Alarm> Alarms { get; }

        private Context _context;

        private Dialog _dayOfWeekDialog;
        private Dialog _timePickerDialog;

        private bool _dialogShowed = false;

        public AlarmAdapter(List<Alarm> alarms, Context context)
        {
            Alarms = alarms;
            _context = context;
        }

        public override int Count => Alarms.Count;

        public override Alarm this[int position] => Alarms[position];

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;

            if (view == null)
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item, parent, false);

            var nfcImage = view.FindViewById<ImageView>(Resource.Id.image_nfc);
            var description = view.FindViewById<TextView>(Resource.Id.text_description);
            var time = view.FindViewById<TextView>(Resource.Id.text_time);
            var daysOfWeek = view.FindViewById<TextView>(Resource.Id.text_dayOfWeek);
            var switchEnabled = view.FindViewById<Android.Support.V7.Widget.SwitchCompat>(Resource.Id.switch_enabled);

            var current = Alarms[position];

            nfcImage.Enabled = current.NfcTagHash == null;
            description.Text = current.Description;

            daysOfWeek.Text = current.GetDaysOfWeekString(parent.Context);
            daysOfWeek.Click += delegate 
            {
                if (!_dialogShowed)
                    ShowDayOfWeekDialog(parent.Context);
            };

            time.Text = current.Time.ToString("hh:mm");
            time.Click += delegate
            {
                if (!_dialogShowed)
                    ShowTimePickerDialog(parent.Context);
            };
            
            switchEnabled.CheckedChange += delegate
            {
                Toast.MakeText(parent.Context, $"OnAlarm[{position}]StateChanged", ToastLength.Short).Show();
            };

            return view;
        }

        private void ShowTimePickerDialog(Context context)
        {
            _dialogShowed = true;
            if (_timePickerDialog == null) CreateTimePickerDialog(context);

            _timePickerDialog.Show();
        }

        private void CreateTimePickerDialog(Context context)
        {
            _timePickerDialog = new TimePickerDialog(
                context, 
                delegate
                {
                    _dialogShowed = false;
                },
                DateTime.Now.Hour,
                DateTime.Now.Minute,
                true);

            _timePickerDialog.CancelEvent += delegate
            {
                _dialogShowed = false;
            };
        }

        public void ShowDayOfWeekDialog(Context context)
        {
            _dialogShowed = true;
            if (_dayOfWeekDialog == null) CreateDayOfWeekDialog(context);

            _dayOfWeekDialog.Show();
        }

        private void CreateDayOfWeekDialog(Context context)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle("Choose some animals");

            string[] animals = { "Понедельник", "Писос", "camel", "sheep", "goat" };
            bool[] checkedItems = { true, false, false, true, false };

            builder.SetMultiChoiceItems(animals, checkedItems, delegate {
                _dialogShowed = false;
            });

            builder.SetPositiveButton("OK", delegate { });
            builder.SetNegativeButton("Cancel", delegate { });

            _dayOfWeekDialog = builder.Create();
            
            _dayOfWeekDialog.DismissEvent += delegate
            {
                _dialogShowed = false;
            };
        }
    }
}