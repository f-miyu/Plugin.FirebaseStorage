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
                Firebase.Storage.FirebaseStorage storage;
                if (string.IsNullOrEmpty(FirebaseStorage.DefaultAppName))
                {
                    storage = Firebase.Storage.FirebaseStorage.Instance;
                }
                else
                {
                    var app = Firebase.FirebaseApp.GetInstance(FirebaseStorage.DefaultAppName);
                    storage = Firebase.Storage.FirebaseStorage.GetInstance(app);
                }
                return new StorageWrapper(storage);
            }
        }

        public IStorage GetInstance(string appName)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(app));
        }

        public IStorage GetInstanceFromUrl(string url)
        {
            Firebase.Storage.FirebaseStorage storage;
            if (string.IsNullOrEmpty(FirebaseStorage.DefaultAppName))
            {
                storage = Firebase.Storage.FirebaseStorage.GetInstance(url);
            }
            else
            {
                var app = Firebase.FirebaseApp.GetInstance(FirebaseStorage.DefaultAppName);
                storage = Firebase.Storage.FirebaseStorage.GetInstance(app, url);
            }
            return new StorageWrapper(storage);
        }

        public IStorage GetInstance(string appName, string url)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(app, url));
        }
    }
}
