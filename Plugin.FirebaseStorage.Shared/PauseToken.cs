using System;
using System.Threading;
using System.Runtime.CompilerServices;
namespace Plugin.FirebaseStorage
{
    public struct PauseToken : IEquatable<PauseToken>
    {
        public static PauseToken None => default(PauseToken);

        private readonly PauseTokenSource _source;

        internal IStorageTask StorageTask
        {
            get => _source?.StorageTask;
            set
            {
                if (_source != null)
                {
                    _source.StorageTask = value;
                }
            }
        }

        public PauseToken(PauseTokenSource source)
        {
            _source = source;
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
