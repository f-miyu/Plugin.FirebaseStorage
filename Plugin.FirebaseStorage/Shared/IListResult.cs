using System;
using System.Collections.Generic;

namespace Plugin.FirebaseStorage
{
    public interface IListResult
    {
        IEnumerable<IStorageReference> Prefixes { get; }
        IEnumerable<IStorageReference> Items { get; }
        string? PageToken { get; }
    }
}
