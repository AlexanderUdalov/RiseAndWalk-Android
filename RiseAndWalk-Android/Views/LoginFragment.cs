using Android.App;
using Android.OS;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace RiseAndWalk_Android.Views
{
    public class LoginFragment : Fragment
    {
        private View _view;

        public event Action OnCreateCallBack;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _view = LayoutInflater.Inflate(Resource.Layout.fragment_login, null);

            var buttonLogin = _view.FindViewById<Button>(Resource.Id.button_login);
            buttonLogin.Click += delegate { OnLoginClicked(); };

            var buttonRegister = _view.FindViewById<Button>(Resource.Id.button_register);
            buttonRegister.Click += delegate { OnRegisterClicked(); };

            OnCreateCallBack?.Invoke();
        }

        public void OnLoginClicked()
        {
            using (var client = new HttpClient())
            {
                Login(client);
            }
        }

        public void OnRegisterClicked()
        {
            using (var client = new HttpClient())
            {
                Register(client);
            }
        }

        private class RegisterData
        {
            public string Email;
            public string Password;
        }

        private void Register(HttpClient client)
        {
            var email = "slasbdhoiash@mail.ru";
            var password = "A1a2A3$fsd";

            var stringContent = new StringContent(
                JsonConvert.SerializeObject(
                    new RegisterData()
                    {
                        Email = email,
                        Password = password
                    }),
                Encoding.UTF8,
                "application/json");
            //var postResponse = client.GetAsync("https://192.168.3.15:45455/api/test").Result;
            var postResponse = client.PostAsync("http://192.168.3.15:5000/api/account/register",
                stringContent).Result;

            Toast.MakeText(_view.Context, "Status code: " + postResponse.StatusCode, ToastLength.Short).Show();
            var content = postResponse.Content.ReadAsStringAsync().Result;

            if (postResponse.IsSuccessStatusCode)
            {
                Toast.MakeText(_view.Context, content, ToastLength.Short).Show();
                var token = content;
                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }

        private void Login(HttpClient client)
        {
            var email = "slasbdhoiash@mail.ru";
            var password = "A1a2A3$fsd";

            var stringContent = new StringContent(
                JsonConvert.SerializeObject(
                    new RegisterData()
                    {
                        Email = email,
                        Password = password
                    }),
                Encoding.UTF8,
                "application/json");

            var postResponse = client.PostAsync("http://192.168.3.15:5000/api/account/login",
                stringContent).Result;

            Toast.MakeText(_view.Context, "Status code: " + postResponse.StatusCode, ToastLength.Short).Show();
            var content = postResponse.Content.ReadAsStringAsync().Result;

            if (postResponse.IsSuccessStatusCode)
            {
                Toast.MakeText(_view.Context, content, ToastLength.Short).Show();
                var token = content;
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    var prefs = PreferenceManager.GetDefaultSharedPreferences(_view.Context);
                    var prefsEditor = prefs.Edit();
                    prefsEditor.PutString("userToken", token);
                    prefsEditor.Commit();
                }
            }
        }

        public void AddCloseButton(Activity closingActivity)
        {
            var viewGroup = (RelativeLayout)_view.FindViewById(Resource.Id.login_container);

            Button bt = new Button(_view.Context);
            bt.SetText("Закрыть это", TextView.BufferType.Normal);
            bt.Click += delegate
            {
                closingActivity.Finish();
            };
            var parametrs = new RelativeLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.WrapContent);
            parametrs.AddRule(LayoutRules.AlignParentRight);
            parametrs.AddRule(LayoutRules.AlignParentBottom);

            viewGroup.AddView(bt, parametrs);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => _view;
    }
}