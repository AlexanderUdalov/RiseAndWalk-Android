using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using RiseAndWalk_Android.Controllers;

namespace RiseAndWalk_Android.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private string _token;
        private Fragment LeftFragment, RightFragment, CenterFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CheckToken();
            CheckRingOutAlarm();
            CheckFirstStart();

            SetContentView(Resource.Layout.activity_main);

            LeftFragment = new AlarmsFragment();
            CenterFragment = new AlarmsFragment();
            RightFragment = string.IsNullOrEmpty(_token) ? (Fragment)new LoginFragment() : new AccountFragment();

            var navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            FragmentManager.BeginTransaction()
                .Add(Resource.Id.fragment_content, new AlarmsFragment())
                .Commit();
        }

        private void CheckToken()
        {
            _token = PreferenceManager.GetDefaultSharedPreferences(this).GetString("userToken", "");
            NetworkController.Instance.SetToken(_token);
        }

        private void CheckRingOutAlarm()
        {
            var ringingAlarmId = Intent.GetStringExtra("ringingAlarmId");
            if (string.IsNullOrEmpty(ringingAlarmId)) return;

            AlarmStoreController.Instance.DeleteOrRepeatRingingAlarm(ringingAlarmId);
        }

        private void CheckFirstStart()
        {
            var pref = PreferenceManager.GetDefaultSharedPreferences(this);
            var firstStart = pref.GetBoolean("firstStart", true);

            if (!firstStart) return;

            var prefEditor = pref.Edit();
            prefEditor.PutBoolean("firstStart", false);
            prefEditor.Commit();

            var intent = new Intent(this, typeof(HelloActivity));
            StartActivity(intent);

        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var transaction = FragmentManager.BeginTransaction();

            switch (item.ItemId)
            {
                case Resource.Id.navigation_alarms:
                    transaction.Replace(Resource.Id.fragment_content, LeftFragment);
                    break;

                case Resource.Id.navigation_time:
                    transaction.Replace(Resource.Id.fragment_content, CenterFragment);
                    break;

                case Resource.Id.navigation_account:
                    transaction.Replace(Resource.Id.fragment_content, RightFragment);
                    break;
            }

            transaction.Commit();
            return true;
        }
    }
}