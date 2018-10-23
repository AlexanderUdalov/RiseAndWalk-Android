using Android.Content;
using Android.Views;
using Android.Widget;
using RiseAndWalk_Android.Controllers;
using RiseAndWalk_Android.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RiseAndWalk_Android.Views
{
    internal class AlarmAdapter : BaseAdapter<Alarm>
    {
        public List<Alarm> Alarms { get; }

        private readonly Context _context;

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
            if (convertView != null) return convertView;
            
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_item, parent, false);

            var nfcImage = view.FindViewById<ImageView>(Resource.Id.image_nfc);
            var description = view.FindViewById<TextView>(Resource.Id.text_description);
            var time = view.FindViewById<TextView>(Resource.Id.text_time);
            var daysOfWeek = view.FindViewById<TextView>(Resource.Id.text_dayOfWeek);
            var switchEnabled = view.FindViewById<Android.Support.V7.Widget.SwitchCompat>(Resource.Id.switch_enabled);
            var switchDeleteAfterRinging = view.FindViewById<Android.Support.V7.Widget.SwitchCompat>(Resource.Id.list_item_switch_delete_after_ringing);


            var current = Alarms[position];

            nfcImage.Visibility = string.IsNullOrEmpty(current.NfcTagHash)? ViewStates.Invisible: ViewStates.Visible;

            description.Text = current.Description;

            daysOfWeek.Text = current.GetDaysOfWeekString(parent.Context);
            daysOfWeek.Click += delegate
            {
                DialogController.Instance.ShowDayOfWeekDialog(_context, (days =>
                    {
                        OnDayOfWeekSet(current, daysOfWeek, days);

                        AlarmStoreController.Instance.UpdateAlarm(current);
                    }));
            };

            time.Text = current.Time.ToString(@"hh\:mm");
            time.Click += delegate
            {
                DialogController.Instance.ShowTimePickerDialog(_context, ((sender, args) =>
                {
                    OnTimeSet(current, time, new TimeSpan(args.HourOfDay, args.Minute, 0));

                    AlarmStoreController.Instance.UpdateAlarm(current);
                }));
            };

            switchDeleteAfterRinging.Checked = current.DeleteAfterRinging;
            switchDeleteAfterRinging.CheckedChange += delegate
            {
                current.DeleteAfterRinging = switchDeleteAfterRinging.Checked;
                AlarmStoreController.Instance.UpdateAlarm(current);
            };

            switchEnabled.Checked = current.Enabled;
            switchEnabled.CheckedChange += delegate
            {
                current.Enabled = switchEnabled.Checked;
                AlarmStoreController.Instance.UpdateAlarm(current);
                if (switchEnabled.Checked)
                    AlarmServiceController.Instance.EnableAlarm(parent.Context, current);
                else
                    AlarmServiceController.Instance.DisableAlarm(parent.Context, current);
            };

            return view;
        }

        private void OnTimeSet(Alarm alarm, TextView timeView, TimeSpan time)
        {
            alarm.Time = time;
            timeView.Text = time.ToString("HH:mm");
        }

        private void OnDayOfWeekSet(Alarm alarm, TextView dayOfWeekView, List<int> choosedItems)
        {
            if (choosedItems.Count > 7) throw new ArgumentException();

            alarm.DaysOfWeek = choosedItems.Cast<DayOfWeek>().ToArray();
            dayOfWeekView.Text = alarm.GetDaysOfWeekString(_context);
        }
    }
}