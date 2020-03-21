using System;
using Firebase.Firestore;
using Android.Runtime;

namespace Plugin.CloudFirestore
{
    public delegate void EventHandler<T>(T value, FirebaseFirestoreException error);

    public class EventHandlerListener<T> : Java.Lang.Object, IEventListener where T : Java.Lang.Object
    {
        private readonly EventHandler<T> _handler;

        public EventHandlerListener(EventHandler<T> handler)
        {
            _handler = handler;
        }

        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            _handler?.Invoke(value.JavaCast<T>(), error);
        }
    }
}
