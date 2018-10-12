using System;
namespace Plugin.FirebaseStorage
{
    public interface IStorage
    {
        IStorageReference GetReferenceFromPath(string path);
        IStorageReference GetReferenceFromUrl(string url);
        IStorageReference GetRootReference();
    }
}
