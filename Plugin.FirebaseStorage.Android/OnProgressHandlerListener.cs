using System;
using Firebase.Storage;
using Java.Lang;
using Android.Runtime;
using Java.Interop;

namespace Plugin.FirebaseStorage
{
    internal delegate void OnProgressHHandler(Java.Lang.Object snapshot);

    internal class OnProgressHandlerListener : Java.Lang.Object, IOnProgressListener
    {
        private OnProgressHHandler _handler;

        public OnProgressHandlerListener(OnProgressHHandler handler)
        {
            _handler = handler;
        }

        public void snapshot(Java.Lang.Object p0)
        {
            _handler?.Invoke(p0);
        }
    }
}
