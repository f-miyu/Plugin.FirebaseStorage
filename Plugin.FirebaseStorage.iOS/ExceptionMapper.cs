using System;
using Foundation;

namespace Plugin.FirebaseStorage
{
    public static class ExceptionMapper
    {
        public static Exception Map(NSErrorException exception)
        {
            return new FirebaseStorageException(exception.Error.LocalizedDescription, exception, ErrorType.Other);
        }
    }
}
