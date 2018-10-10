using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using RiseAndWalk_Android.Models;
using RiseAndWalk_Android.ViewModels;
using System.Linq;
using Android.Content;

namespace RiseAndWalk_Android.Views
{
    public class AlarmsFragment : Fragment
    {
        private AlarmsViewModel _viewModel;
        private View _view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = new AlarmsViewModel();

            _view = LayoutInflater.Inflate(Resource.Layout.fragment_alarms, null);

            var fabAddNewAlarm = _view.FindViewById(Resource.Id.fab_add);
            fabAddNewAlarm.Click += delegate { OnAddAlarmClicked(); };

            var alarmsView = _view.FindViewById<ListView>(Resource.Id.alarms_list);

            var alarmsList = _viewModel.Alarms
                    .GetItemsAsync()
                    .GetAwaiter()
                    .GetResult()
                    .ToList();

            alarmsView.Adapter = new AlarmAdapter(alarmsList, Activity);
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => _view;

        public void OnAddAlarmClicked()
        {
            var intent = new Intent(Context, typeof(AddNewAlarmActivity));
            StartActivity(intent);
        }
    }
}