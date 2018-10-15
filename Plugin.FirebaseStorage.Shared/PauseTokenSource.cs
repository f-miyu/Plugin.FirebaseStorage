using System;
namespace Plugin.FirebaseStorage
{
    public class PauseTokenSource
    {
        internal IStorageTask StorageTask { get; set; }

        public bool IsPuased => StorageTask?.IsPaused ?? false;

        public void Pause()
        {
            StorageTask?.Puase();
        }

        public void Resume()
        {
            StorageTask?.Resume();
        }
    }
}
