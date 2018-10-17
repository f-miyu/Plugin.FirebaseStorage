using System;
namespace Plugin.FirebaseStorage
{
    public interface IStorage
    {
        IStorageReference RootReference { get; }
        IStorageReference GetReferenceFromPath(string path);
        IStorageReference GetReferenceFromUrl(string url);
    }
}
