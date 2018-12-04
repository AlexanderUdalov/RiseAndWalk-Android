using Android.Content;
using Android.Views;
using Android.Widget;
using RiseAndWalk_Android.Controllers;
using RiseAndWalk_Android.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V7.Widget;

namespace RiseAndWalk_Android.Views
{
    internal class AlarmAdapter : RecyclerView.Adapter
    {
        public List<Alarm> Alarms { get; }

        public class AlarmViewHolder : RecyclerView.ViewHolder
        {
            public ImageView NfcImage { get; set; }
            public TextView Description { get; set; }
            public TextView Time { get; set; }
            public TextView DaysOfWeek { get; set; }
            public SwitchCompat SwitchEnabled { get; set; }
            public SwitchCompat SwitchDeleteAfterRinging { get; set; }
            public Context Context { get; set; }

            public AlarmViewHolder(View view) : base(view)
            {
                Context = view.Context;
                   NfcImage = view.FindViewById<ImageView>(Resource.Id.image_nfc);
                Description = view.FindViewById<TextView>(Resource.Id.text_description);
                Time = view.FindViewById<TextView>(Resource.Id.text_time);
                DaysOfWeek = view.FindViewById<TextView>(Resource.Id.text_dayOfWeek);
                SwitchEnabled =
                    view.FindViewById<SwitchCompat>(Resource.Id.switch_enabled);
                SwitchDeleteAfterRinging =
                    view.FindViewById<SwitchCompat>(Resource.Id.list_item_switch_delete_after_ringing);
            }
        }

        public AlarmAdapter(List<Alarm> alarms)
        {
            Alarms = new List<Alarm>();
            Alarms.Add(new Alarm
            {
                DaysOfWeek =  null,
                DeleteAfterRinging = true,
                Description = "pisos"
            });
            Alarms.Add(new Alarm
            {
                DaysOfWeek = null,
                DeleteAfterRinging = true,
                Description = "pisos1"
            });
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var alarm = holder as AlarmViewHolder;
            var current = Alarms[position];

            alarm.NfcImage.Visibility = string.IsNullOrEmpty(current.NfcTagHash) ? ViewStates.Invisible : ViewStates.Visible;

            alarm.Description.Text = current.Description;

            alarm.DaysOfWeek.Text = current.GetDaysOfWeekString(alarm.Context);
            alarm.DaysOfWeek.Click += delegate
            {
                DialogController.Instance.ShowDayOfWeekDialog(alarm.Context, (days =>
                    {
                        OnDayOfWeekSet(current, alarm.DaysOfWeek, days, alarm.Context);

                        AlarmStoreController.Instance.UpdateAlarm(current);
                    }));
            };

            alarm.Time.Text = current.Time.ToString(@"hh\:mm");
            alarm.Time.Click += delegate
            {
                DialogController.Instance.ShowTimePickerDialog(alarm.Context, ((sender, args) =>
                {
                    OnTimeSet(current, alarm.Time, new TimeSpan(args.HourOfDay, args.Minute, 0));

                    AlarmStoreController.Instance.UpdateAlarm(current);
                }));
            };

            alarm.SwitchDeleteAfterRinging.Checked = current.DeleteAfterRinging;
            alarm.SwitchDeleteAfterRinging.CheckedChange += delegate
            {
                current.DeleteAfterRinging = alarm.SwitchDeleteAfterRinging.Checked;
                AlarmStoreController.Instance.UpdateAlarm(current);
            };

            alarm.SwitchEnabled.Checked = current.Enabled;
            alarm.SwitchEnabled.CheckedChange += delegate
            {
                current.Enabled = alarm.SwitchEnabled.Checked;
                AlarmStoreController.Instance.UpdateAlarm(current);
                if (alarm.SwitchEnabled.Checked)
                    AlarmServiceController.Instance.EnableAlarm(alarm.Context, current);
                else
                    AlarmServiceController.Instance.DisableAlarm(alarm.Context, current);
            };
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.list_item, parent, false);

            return new AlarmViewHolder(itemView);
        }

        public override int ItemCount => Alarms.Count;


        private void OnTimeSet(Alarm alarm, TextView timeView, TimeSpan time)
        {
            alarm.Time = time;
            timeView.Text = time.ToString("HH:mm");
        }

        private void OnDayOfWeekSet(Alarm alarm, TextView dayOfWeekView, List<int> choosedItems, Context context)
        {
            if (choosedItems.Count > 7) throw new ArgumentException();

            alarm.DaysOfWeek = choosedItems.Cast<DayOfWeek>().ToArray();
            dayOfWeekView.Text = alarm.GetDaysOfWeekString(context);
        }
    }
}
