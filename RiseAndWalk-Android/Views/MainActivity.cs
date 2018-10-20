using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;

namespace RiseAndWalk_Android.Views
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private ISharedPreferencesEditor _prefsEditor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            _prefsEditor = prefs.Edit();

            var firstStart = prefs.GetBoolean("firstStart", true);

            if (firstStart)
            {
                var intent = new Intent(this, typeof(HelloActivity));
                StartActivity(intent);
            }

            _prefsEditor.PutBoolean("firstStart", false);
            _prefsEditor.Commit();

            SetContentView(Resource.Layout.activity_main);

            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            FragmentManager.BeginTransaction()
                .Add(Resource.Id.fragment_content, new AlarmsFragment())
                .Commit();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            var transaction = FragmentManager.BeginTransaction();

            switch (item.ItemId)
            {
                case Resource.Id.navigation_alarms:
                    transaction.Replace(Resource.Id.fragment_content, new AlarmsFragment());
                    break;

                case Resource.Id.navigation_time:
                    transaction.Replace(Resource.Id.fragment_content, new AlarmsFragment());
                    break;

                case Resource.Id.navigation_account:
                    var token = PreferenceManager.GetDefaultSharedPreferences(this).GetString("userToken", "");
                    transaction.Replace(Resource.Id.fragment_content,
                        string.IsNullOrEmpty(token) ? (Fragment)new LoginFragment() : new AccountFragment());
                    break;
            }

            transaction.Commit();
            return true;
        }
    }
}