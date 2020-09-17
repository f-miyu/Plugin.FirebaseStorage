using System;

namespace Plugin.FirebaseStorage
{
    public struct PauseToken : IEquatable<PauseToken>
    {
        public static PauseToken None => default;

        private readonly PauseTokenSource _source;

        public PauseToken(PauseTokenSource source)
        {
            _source = source;
        }

        internal void SetStorageTask(IStorageTask storageTask)
        {
            if (_source != null)
            {
                _source.StorageTask = storageTask;
            }
        }

        public bool Equals(PauseToken other)
        {
            if (_source == null && other._source == null)
            {
                return true;
            }
            return _source == other._source;
        }

        public override bool Equals(object obj)
        {
            if (obj is PauseToken pauseToken)
            {
                return Equals(pauseToken);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _source?.GetHashCode() ?? 0;
        }

        public static bool operator ==(PauseToken left, PauseToken right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PauseToken left, PauseToken right)
        {
            return !left.Equals(right);
        }
    }
}
