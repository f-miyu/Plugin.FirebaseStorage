using System;
namespace Plugin.FirebaseStorage
{
    public class PauseTokenSource
    {
        internal event EventHandler Paused;
        internal event EventHandler Resumed;

        public void Pause()
        {
            Paused?.Invoke(this, EventArgs.Empty);
        }

        public void Resume()
        {
            Resumed?.Invoke(this, EventArgs.Empty);
        }
    }
}
