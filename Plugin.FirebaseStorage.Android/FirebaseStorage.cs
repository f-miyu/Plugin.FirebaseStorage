using System;
using Android.Content;

namespace Plugin.FirebaseStorage
{
    public static class FirebaseStorage
    {
        public static readonly string DefaultAppName = "[FirebasePlugin]";

        public static void Init(Context context)
        {
            try
            {
                Firebase.FirebaseApp.GetInstance(DefaultAppName);
            }
            catch (Exception)
            {
                var baseOptions = Firebase.FirebaseOptions.FromResource(context);
                var options = new Firebase.FirebaseOptions.Builder(baseOptions).SetProjectId(baseOptions.StorageBucket.Split('.')[0]).Build();

                Firebase.FirebaseApp.InitializeApp(context, options, DefaultAppName);
            }
        }
    }
}
