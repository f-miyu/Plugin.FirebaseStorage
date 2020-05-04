using System;
namespace Plugin.FirebaseStorage
{
    public interface IStorage
    {
        IStorageReference RootReference { get; }
        TimeSpan MaxDownloadRetryTime { get; set; }
        TimeSpan MaxOperationRetryTime { get; set; }
        TimeSpan MaxUploadRetryTime { get; set; }
        IStorageReference GetReferenceFromPath(string path);
        IStorageReference GetReferenceFromUrl(string url);
    }
}
