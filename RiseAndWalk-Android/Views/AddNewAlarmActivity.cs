using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Nfc;
using Android.Nfc.Tech;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RiseAndWalk_Android.Models;

namespace RiseAndWalk_Android.Views
{
    [Activity(Label = "AddNewAlarmActivity")]
    public class AddNewAlarmActivity : Activity
    {
        public static String MIME_TEXT_PLAIN = "text/plain";
        public static String TAG = "NfcDemo";

        private Dialog _dayOfWeekDialog;
        private Dialog _timePickerDialog;

        private Alarm _newAlarm;

        private bool _dialogShowed = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_new_alarm);


            var description = FindViewById<TextView>(Resource.Id.text_input_description);
            var daysOfWeek = FindViewById<TextView>(Resource.Id.picker_day_of_week);
            var time = FindViewById<TextView>(Resource.Id.picker_time);
            var addNfcButton = FindViewById<Button>(Resource.Id.button_add_nfc);
            var saveButton = FindViewById(Resource.Id.fab_save);

            daysOfWeek.Click += delegate
            {
                if (!_dialogShowed)
                    ShowDayOfWeekDialog(this);
            };

            time.Text = DateTime.Now.ToString("hh:mm");
            time.Click += delegate
            {
                if (!_dialogShowed)
                    ShowTimePickerDialog(this);
            };

            addNfcButton.Click += delegate { OnAddNfcClicked(); };
            saveButton.Click += delegate { OnSaveClicked(); };
        }


        private void OnAddNfcClicked()
        {
            Toast.MakeText(this, "Add nfc", ToastLength.Short).Show();
            var intent = new Intent(this, typeof(NfcActivity));
            StartActivity(intent);
        }

        private void OnSaveClicked()
        {
            Toast.MakeText(this, "Save", ToastLength.Short).Show();
            Finish();
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