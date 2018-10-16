using System;
using Firebase;
using Firebase.Storage;

namespace Plugin.FirebaseStorage
{
    public static class ExceptionMapper
    {
        public static Exception Map(Exception exception)
        {
            return new FirebaseStorageException(exception.Message, exception, ErrorType.Unkown);
        }
    }
}