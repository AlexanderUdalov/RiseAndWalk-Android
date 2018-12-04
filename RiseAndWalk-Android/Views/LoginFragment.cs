using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using RiseAndWalk_Android.Controllers;
using RiseAndWalk_Android.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Android.Support.Transitions;

namespace RiseAndWalk_Android.Views
{
    public class LoginFragment : Fragment
    {
        private View _view;
        private EditText _email;
        private EditText _password;

        private Button _login, _register;

        public event Action OnCreateCallBack;

        public event Action OnAuthorizedCallBack;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _view = LayoutInflater.Inflate(Resource.Layout.fragment_login, null);
            
            _email = _view.FindViewById<EditText>(Resource.Id.fragment_login_email_field);
            _password = _view.FindViewById<EditText>(Resource.Id.fragment_login_password_field);

            _login = _view.FindViewById<Button>(Resource.Id.button_login);
            _login.Click += OnLoginClicked;

            _register = _view.FindViewById<Button>(Resource.Id.button_register);
            _register.Click += OnRegisterClicked;
            
            OnCreateCallBack?.Invoke();
        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            _register.Enabled = false;
            _login.Enabled = false;
            var token = await NetworkController.Instance.LoginAsync(new UserModel
            {
                Email = _email.Text,
                Password = _password.Text
            });

            if (!string.IsNullOrEmpty(token))
                OnAuthorizationSuccess(token);
            else
                OnAuthorizationError();
        }

        public async void OnRegisterClicked(object sender, EventArgs e)
        {
            _register.Enabled = false;
            _login.Enabled = false;
            var token = await NetworkController.Instance.RegisterAsync(new UserModel
            {
                Email = _email.Text,
                Password = _password.Text
            });

            if (!string.IsNullOrEmpty(token))
                OnAuthorizationSuccess(token);
            else
                OnAuthorizationError();
        }

        public void OnAuthorizationSuccess(string token)
        {
            Toast.MakeText(Context, "Успешно", ToastLength.Short).Show();

            NetworkController.Instance.SetToken(token);
            NetworkController.Instance.SaveToken(Context, token);

            OnAuthorizedCallBack?.Invoke();
        }
        
        public void OnAuthorizationError()
        {
            Toast.MakeText(Context, "Ошибка", ToastLength.Short).Show();
            _password.Error= "Неверный пароль!";
            _register.Enabled = true;
            _login.Enabled = true;
        }

        public void AddCloseButton(Activity closingActivity)
        {
            var viewGroup = (RelativeLayout)_view.FindViewById(Resource.Id.fragment_login_container);

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