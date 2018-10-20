using Android.App;
using Android.OS;
using Android.Views;

namespace RiseAndWalk_Android.Views
{
    public class AccountFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
           => base.OnCreate(savedInstanceState);

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.fragment_account, container, false);
    }
}