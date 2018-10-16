using System;
using System.Collections.Generic;
namespace Plugin.FirebaseStorage
{
    public class MetadataChange
    {
        private string _cacheControl;
        public string CacheControl
        {
            get => _cacheControl;
            set
            {
                _cacheControl = value;
                IsCacheControlChanged = true;
            }
        }

        private string _contentDisposition;
        public string ContentDisposition
        {
            get => _contentDisposition;
            set
            {
                _contentDisposition = value;
                IsContentDispositionChanged = true;
            }
        }

        private string _contentEncoding;
        public string ContentEncoding
        {
            get => _contentEncoding;
            set
            {
                _contentEncoding = value;
                IsContentEncodingChanged = true;
            }
        }

        private string _contentLanguage;
        public string ContentLanguage
        {
            get => _contentLanguage;
            set
            {
                _contentLanguage = value;
                IsContentLanguageChanged = true;
            }
        }

        private string _contentType;
        public string ContentType
        {
            get => _contentType;
            set
            {
                _contentType = value;
                IsContentTypeChanged = true;
            }
        }

        IDictionary<string, string> CustomMetadata { get; set; }

        internal bool IsCacheControlChanged { get; private set; }
        internal bool IsContentDispositionChanged { get; private set; }
        internal bool IsContentEncodingChanged { get; private set; }
        internal bool IsContentLanguageChanged { get; private set; }
        internal bool IsContentTypeChanged { get; private set; }
    }
}
