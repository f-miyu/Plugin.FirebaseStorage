using System;
namespace Plugin.FirebaseStorage
{
    public static class FirebaseStorage
    {
        public static void Init()
        {
            Firebase.Core.App.Configure();
        }
    }
}
