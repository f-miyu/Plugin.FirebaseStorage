using System;
namespace Plugin.FirebaseStorage
{
    public class FirebaseStorageImplementation : IFirebaseStorage
    {
        private readonly Firebase.Storage.FirebaseStorage _instance = Firebase.Storage.FirebaseStorage.Instance;

        public FirebaseStorageImplementation()
        {
        }
    }
}
