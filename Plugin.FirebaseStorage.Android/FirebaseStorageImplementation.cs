using System;
using System.Collections.Generic;
namespace Plugin.FirebaseStorage
{
    public class FirebaseStorageImplementation : IFirebaseStorage
    {
        public IStorage Storage => new StorageWrapper(Firebase.Storage.FirebaseStorage.Instance);

        public IStorage GetStorage(string url)
        {
            if (url == null)
                throw new ArgumentNullException(nameof(url));

            return new StorageWrapper(Firebase.Storage.FirebaseStorage.GetInstance(url));
        }
    }
}
