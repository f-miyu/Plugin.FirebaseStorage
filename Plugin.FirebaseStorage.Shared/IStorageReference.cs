using System;
using System.IO;
namespace Plugin.FirebaseStorage
{
    public interface IStorageReference
    {
        string FullPath { get; }
        IStorageReference GetChild(string path);
        Stream GetDataAsync();

    }
}
