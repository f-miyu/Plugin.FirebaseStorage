using System;
using System.Collections.Generic;
namespace Plugin.FirebaseStorage
{
    public class FirebaseStorageImplementation : IFirebaseStorage
    {
        public IStorage Instance
        {
            get
            {
                var app = Firebase.FirebaseApp.GetInstance(FirebaseStorage.DefaultAppName);
                return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(app));
            }
        }

        public IStorage GetInstance(string appName)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(app));
        }

        public IStorage GetInstanceFromUrl(string url)
        {
            var app = Firebase.FirebaseApp.GetInstance(FirebaseStorage.DefaultAppName);
            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(app, url));
        }

        public IStorage GetInstance(string appName, string url)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(app, url));
        }
    }
}
