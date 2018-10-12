using System;
namespace Plugin.FirebaseStorage
{
    public interface IFirebaseStorage
    {
        IStorage Storage { get; }
        IStorage GetStorage(string url);
    }
}
