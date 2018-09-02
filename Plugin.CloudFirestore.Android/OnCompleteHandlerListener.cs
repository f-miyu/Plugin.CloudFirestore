using System;
using Android.Gms.Tasks;
namespace Plugin.CloudFirestore
{
    public delegate void OnCompleteHandler(Task task);

    public class OnCompleteHandlerListener : Java.Lang.Object, IOnCompleteListener
    {
        private OnCompleteHandler _handler;

        public OnCompleteHandlerListener(OnCompleteHandler handler)
        {
            _handler = handler;
        }

        public void OnComplete(Task task)
        {
            _handler?.Invoke(task);
        }
    }
}