using System;
namespace Plugin.FirebaseStorage
{
    public struct PauseToken
    {
        private PauseTokenSource _source;

        public PauseToken(PauseTokenSource source)
        {
            _source = source;
        }

        public void Register(Action onPaused, Action onResumed)
        {

        }
    }
}
