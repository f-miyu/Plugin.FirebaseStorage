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
                return new StorageWrapper(Firebase.Storage.FirebaseStorage.Instance);
            }
        }

        public IStorage GetInstance(string appName)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(app));
        }

        public IStorage GetInstanceFromUrl(string url)
        {
            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(url));
        }

        public IStorage GetInstance(string appName, string url)
        {
            var app = Firebase.FirebaseApp.GetInstance(appName);
            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(app, url));
        }
    }
}
