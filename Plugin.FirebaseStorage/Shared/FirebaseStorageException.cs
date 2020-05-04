using System;
namespace Plugin.FirebaseStorage
{
    public class FirebaseStorageException : Exception
    {
        public ErrorType ErrorType { get; }

        public FirebaseStorageException(string message, ErrorType errorType) : base(message)
        {
            ErrorType = errorType;
        }

        public FirebaseStorageException(string message, Exception innerException, ErrorType errorType) : base(message, innerException)
        {
            ErrorType = errorType;
        }
    }
}
