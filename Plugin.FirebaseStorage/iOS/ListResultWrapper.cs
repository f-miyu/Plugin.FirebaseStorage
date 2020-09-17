using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public class ListResultWrapper : IListResult, IEquatable<ListResultWrapper>
    {
        private readonly StorageListResult _storageListResult;

        public ListResultWrapper(StorageListResult storageListResult)
        {
            _storageListResult = storageListResult ?? throw new ArgumentNullException(nameof(storageListResult));
        }

        public IEnumerable<IStorageReference> Prefixes => _storageListResult.Prefixes.Select(reference => new StorageReferenceWrapper(reference));

        public IEnumerable<IStorageReference> Items => _storageListResult.Items.Select(reference => new StorageReferenceWrapper(reference));

        public string? PageToken => _storageListResult.PageToken;

        public override bool Equals(object? obj)
        {
            return Equals(obj as ListResultWrapper);
        }

        public bool Equals(ListResultWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_storageListResult, other._storageListResult)) return true;
            return _storageListResult.Equals(other._storageListResult);
        }

        public override int GetHashCode()
        {
            return _storageListResult.GetHashCode();
        }
    }
}
