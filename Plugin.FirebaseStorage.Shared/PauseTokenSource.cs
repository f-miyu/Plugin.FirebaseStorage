using System;
namespace Plugin.FirebaseStorage
{
    public class PauseTokenSource
    {
        internal IStorageTask StorageTask { get; set; }

        public PauseToken Token => new PauseToken(this);

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
