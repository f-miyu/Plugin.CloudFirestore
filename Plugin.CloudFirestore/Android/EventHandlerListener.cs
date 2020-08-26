using System;
using Firebase.Firestore;
using Android.Runtime;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.CloudFirestore
{
    internal delegate void EventHandler<T>([AllowNull] T value, FirebaseFirestoreException? error);

    internal class EventHandlerListener<T> : Java.Lang.Object, IEventListener where T : Java.Lang.Object
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
