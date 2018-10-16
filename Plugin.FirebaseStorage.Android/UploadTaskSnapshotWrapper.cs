using System;
using Firebase.Storage;
namespace Plugin.FirebaseStorage
{
    public class UploadTaskSnapshotWrapper : IUploadTaskSnapshot
    {
        internal UploadTask.TaskSnapshot TaskSnapshot { get; }

        public long BytesTransferred => TaskSnapshot.BytesTransferred;

        public long TotalByteCount => TaskSnapshot.TotalByteCount;

        public UploadTaskSnapshotWrapper(UploadTask.TaskSnapshot taskSnapshot)
        {
            TaskSnapshot = taskSnapshot;
        }
    }
}
