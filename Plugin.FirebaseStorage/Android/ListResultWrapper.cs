using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class ListResultWrapper : IListResult, IEquatable<ListResultWrapper>
    {
        private readonly ListResult _listResult;

        public ListResultWrapper(ListResult listResult)
        {
            _listResult = listResult ?? throw new ArgumentNullException(nameof(listResult));
        }

        public IEnumerable<IStorageReference> Prefixes => _listResult.Prefixes.Select(reference => new StorageReferenceWrapper(reference));

        public IEnumerable<IStorageReference> Items => _listResult.Items.Select(reference => new StorageReferenceWrapper(reference));

        public string? PageToken => _listResult.PageToken;

        public override bool Equals(object? obj)
        {
            return Equals(obj as ListResultWrapper);
        }

        public bool Equals(ListResultWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_listResult, other._listResult)) return true;
            return _listResult.Equals(other._listResult);
        }

        public override int GetHashCode()
        {
            return _listResult.GetHashCode();
        }
    }
}
