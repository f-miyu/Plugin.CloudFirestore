using System;
namespace Plugin.CloudFirestore
{
    public class ListenerRegistrationWrapper : IListenerRegistration
    {
        private Firebase.Firestore.IListenerRegistration ListenerRegistration { get; }

        public ListenerRegistrationWrapper(Firebase.Firestore.IListenerRegistration listenerRegistration)
        {
            ListenerRegistration = listenerRegistration;
        }

        public void Remove()
        {
            ListenerRegistration.Remove();
        }
    }
}
