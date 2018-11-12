using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Widget;
using RiseAndWalk_Android.Controllers;

namespace RiseAndWalk_Android.Views
{
    public class SwipeRefreshFragment : Fragment
    {
        private View _view;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _view = LayoutInflater.Inflate(Resource.Layout.fragment_swipe_refresh, null);

            var swipeRefreshLayout = (SwipeRefreshLayout)_view.FindViewById(Resource.Id.swiperefresh);
            swipeRefreshLayout.Refresh += delegate { AlarmStoreController.Instance.UpdateStoreAsync(); };
            
            var alarmsView = _view.FindViewById<ListView>(Resource.Id.alarms_list);

            alarmsView.Adapter = GetNewAlarmAdapter();
            AlarmStoreController.Instance.OnDataStoreChanged += () => alarmsView.Adapter = GetNewAlarmAdapter();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => _view;
        
        private AlarmAdapter GetNewAlarmAdapter()
        {
            var alarmsList = AlarmStoreController.Instance.GetAlarms();

            return new AlarmAdapter(alarmsList.ToList(), Activity);
        }
    }
}