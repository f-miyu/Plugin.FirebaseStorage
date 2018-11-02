using System;
namespace Plugin.FirebaseStorage
{
    public interface IFirebaseStorage
    {
        IStorage Instance { get; }
        IStorage GetInstance(string appName);
        IStorage GetInstanceFromUrl(string url);
        IStorage GetInstance(string appName, string url);
    }
}
