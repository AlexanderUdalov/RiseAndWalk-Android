using Android.App;
using Android.OS;

namespace RiseAndWalk_Android.Views
{
    [Activity(Label = "HelloActivity")]
    public class HelloActivity : Activity
    {
        private const string FragmentTag = "HelloFragment";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_hello);

            var fragment = new LoginFragment();
            fragment.OnCreateCallBack += () => fragment.AddCloseButton(this);

            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.hello_content, fragment, FragmentTag);
            transaction.Commit();
        }
    }
}