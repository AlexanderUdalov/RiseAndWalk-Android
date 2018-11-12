﻿using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using RiseAndWalk_Android.Controllers;
using RiseAndWalk_Android.Models;
using System;

namespace RiseAndWalk_Android.Views
{
    public class LoginFragment : Fragment
    {
        private View _view;
        private TextView _email;
        private TextView _password;

        public event Action OnCreateCallBack;

        public event Action OnAuthorizedCallBack;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _view = LayoutInflater.Inflate(Resource.Layout.fragment_login, null);

            _email = _view.FindViewById<TextView>(Resource.Id.fragment_login_email_field);
            _password = _view.FindViewById<TextView>(Resource.Id.fragment_login_password_field);

            var buttonLogin = _view.FindViewById<Button>(Resource.Id.button_login);
            buttonLogin.Click += OnLoginClicked;

            var buttonRegister = _view.FindViewById<Button>(Resource.Id.button_register);
            buttonRegister.Click += OnRegisterClicked;

            OnCreateCallBack?.Invoke();
        }

        public async void OnLoginClicked(object sender, EventArgs e)
        {
            var token = await NetworkController.Instance.LoginAsync(new UserModel
            {
                Email = _email.Text,
                Password = _password.Text
            });

            if (!string.IsNullOrEmpty(token))
            {
                Toast.MakeText(Context, token, ToastLength.Long);
                NetworkController.Instance.SetToken(token);
                NetworkController.Instance.SaveToken(Context, token);
            }
            OnAuthorizedCallBack?.Invoke();
        }

        public async void OnRegisterClicked(object sender, EventArgs e)
        {
            var token = await NetworkController.Instance.RegisterAsync(new UserModel
            {
                Email = _email.Text,
                Password = _password.Text
            });

            if (!string.IsNullOrEmpty(token))
            {
                Toast.MakeText(Context, token, ToastLength.Long);
                NetworkController.Instance.SetToken(token);
                NetworkController.Instance.SaveToken(Context, token);
            }
            OnAuthorizedCallBack?.Invoke();
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