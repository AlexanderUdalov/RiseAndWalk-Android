using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using RiseAndWalk_Android.Controllers;
using RiseAndWalk_Android.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RiseAndWalk_Android.Views
{
    //TODO: maybe change to async?
    [Activity(Label = "AddNewAlarmActivity")]
    public class AddNewAlarmActivity : Activity
    {
        private Button _buttonAddNfc;
        private TextView _description, _time, _daysOfWeek;

        private Alarm _newAlarm;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_new_alarm);

            _newAlarm = new Alarm();

            _description = FindViewById<TextView>(Resource.Id.text_input_description);

            _buttonAddNfc = FindViewById<Button>(Resource.Id.button_add_nfc);
            _buttonAddNfc.Click += delegate { OnAddNfcClicked(); };

            var saveButton = FindViewById(Resource.Id.fab_save);
            saveButton.Click += delegate { OnSaveClicked(); };

            _daysOfWeek = FindViewById<TextView>(Resource.Id.picker_day_of_week);
            _daysOfWeek.Click += delegate { DialogController.Instance.ShowDayOfWeekDialog(this, OnDayOfWeekSet); };

            _time = FindViewById<TextView>(Resource.Id.picker_time);
            _time.Text = DateTime.Now.ToString("HH:mm");
            _time.Click += delegate { DialogController.Instance.ShowTimePickerDialog(this, OnTimeSet); };
        }

        private void OnAddNfcClicked()
        {
            var intent = new Intent(this, typeof(NfcActivity));
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode != Result.Ok) return;

            _newAlarm.NfcTagHash = data.GetStringExtra("nfcTagHash");
            _buttonAddNfc.Text = GetString(Resource.String.change_nfc_tag);
        }

        private void OnSaveClicked()
        {
            _newAlarm.Description = _description.Text;
            _newAlarm.Id = Guid.NewGuid();
            _newAlarm.Enabled = true;

            AlarmStoreController.Instance.DataStore.AddItemAsync(_newAlarm).Wait();

            AlarmServiceController.Instance.EnableAlarm(this, _newAlarm);
            Finish();
        }

        private void OnTimeSet(object sender, TimePickerDialog.TimeSetEventArgs args)
        {
            _newAlarm.Time = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                args.HourOfDay,
                args.Minute, 0);

            _time.Text = _newAlarm.Time.ToString("HH:mm");
        }

        private void OnDayOfWeekSet(List<int> choosedItems)
        {
            if (choosedItems.Count > 7) throw new ArgumentException();

            _newAlarm.DaysOfWeek = choosedItems.Cast<DayOfWeek>().ToArray();

            _daysOfWeek.Text = _newAlarm.GetDaysOfWeekString(this);
        }
    }
}