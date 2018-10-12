using System;
using System.Collections.Generic;
namespace Plugin.FirebaseStorage
{
    public interface IStorageMetadata
    {
        string Name { get; }
        string Path { get; }
        long Size { get; }
        string CacheControl { get; }
        string ContentDisposition { get; }
        string ContentEncoding { get; }
        string ContentLanguage { get; }
        string ContentType { get; }
        Dictionary<string, string> CustomMetadata { get; set; }
    }
}
