using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;

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

            FragmentManager.BeginTransaction()
                .Add(Resource.Id.fragment_alarms_content, new SwipeRefreshFragment())
                .Commit();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => _view;

        private void OnAddAlarmClicked()
        {
            var intent = new Intent(Context, typeof(AddNewAlarmActivity));
            StartActivity(intent);
        }
    }
}