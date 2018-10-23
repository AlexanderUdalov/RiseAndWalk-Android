using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using RiseAndWalk_Android.Controllers;
using System.Linq;
using SQLite;

namespace RiseAndWalk_Android.Views
{
    public class AlarmsFragment : Fragment
    {
        private View _view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _view = LayoutInflater.Inflate(Resource.Layout.fragment_alarms, null);

            var fabAddNewAlarm = _view.FindViewById(Resource.Id.fab_add);
            fabAddNewAlarm.Click += delegate { OnAddAlarmClicked(); };

            var alarmsView = _view.FindViewById<ListView>(Resource.Id.alarms_list);


            alarmsView.Adapter = GetNewAlarmAdapter();
            AlarmStoreController.Instance.OnDataStoreChanged += () => alarmsView.Adapter = GetNewAlarmAdapter();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => _view;

        private void OnAddAlarmClicked()
        {
            var intent = new Intent(Context, typeof(AddNewAlarmActivity));
            StartActivity(intent);
        }
        
        private AlarmAdapter GetNewAlarmAdapter()
        {
            var alarmsList = AlarmStoreController.Instance.GetAlarms();

            return new AlarmAdapter(alarmsList.ToList(), Activity);
        }
    }
}