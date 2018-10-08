using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace RiseAndWalk_Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);

            var transaction = FragmentManager.BeginTransaction();
            transaction.Add(Resource.Id.fragment_content, new AlarmsFragment());

            transaction.Commit();
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
                    transaction.Replace(Resource.Id.fragment_content, new AccountFragment());
                    break;
            }

            transaction.Commit();
            return true;
        }
    }
}

