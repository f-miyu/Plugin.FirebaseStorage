using System;
namespace Plugin.FirebaseStorage
{
    public interface IFirebaseStorage
    {
        IStorage Storage { get; }
        IStorage GetStorage(string appName);
        IStorage GetStorageFromUrl(string url);
        IStorage GetStorage(string appName, string url);
    }
}
