﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace RiseAndWalk_Android
{
    public class AccountFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
           => base.OnCreate(savedInstanceState);


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            => inflater.Inflate(Resource.Layout.account_fragment, container, false);
    }
}