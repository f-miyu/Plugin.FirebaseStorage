using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class UploadTaskWrapper : IUploadTask
    {
        internal StorageUploadTask UploadTask { get; }

        public UploadTaskWrapper(StorageUploadTask uploadTask)
        {
            UploadTask = uploadTask;
        }
    }
}
