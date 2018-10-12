using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class StorageReferenceWrapper : IStorageReference
    {
        internal StorageReference StorageReference { get; }

        public StorageReferenceWrapper(StorageReference storageReference)
        {
            StorageReference = storageReference;
        }
    }
}
