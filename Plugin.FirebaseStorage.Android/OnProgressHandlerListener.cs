﻿using System;
using Firebase.Storage;
using Java.Lang;
using Android.Runtime;
using Java.Interop;

namespace Plugin.FirebaseStorage
{
    public delegate void OnProgressHHandler(Java.Lang.Object snapshot);

    public class OnProgressHandlerListener : Java.Lang.Object, IOnProgressListener
    {
        private OnProgressHHandler _handler;

        public OnProgressHandlerListener(OnProgressHHandler handler)
        {
            _handler = handler;
        }

        public void OnProgress(Java.Lang.Object snapshot)
        {
            _handler?.Invoke(snapshot);
        }
    }
}