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

namespace RiseAndWalk_Android
{
    public class AlarmsFragment : Fragment
    {
        private AlarmsViewModel _viewModel;
        private View _view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _viewModel = new AlarmsViewModel();

            _view = LayoutInflater.Inflate(Resource.Layout.alarms_fragment, null);

            var fabAddNewAlarm = _view.FindViewById(Resource.Id.fab_add);
            fabAddNewAlarm.Click += delegate { OnAddAlarmClicked(); };

            var alarmsView = _view.FindViewById<ListView>(Resource.Id.alarms_list);
            alarmsView.Adapter = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleListItem1,
                _viewModel.Alarms
                    .GetItemsAsync()
                    .GetAwaiter()
                    .GetResult()
                    .ToList());
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => _view;
        
        public void OnAddAlarmClicked()
        {
            Toast.MakeText(Context, "Add alarm", ToastLength.Long).Show();
        }
    }
}