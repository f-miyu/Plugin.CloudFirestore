using System;
namespace Plugin.CloudFirestore
{
    public class ListenerRegistrationWrapper : IListenerRegistration
    {
        private readonly Firebase.Firestore.IListenerRegistration _listenerRegistration;

        public ListenerRegistrationWrapper(Firebase.Firestore.IListenerRegistration listenerRegistration)
        {
            _listenerRegistration = listenerRegistration;
        }

        public void Remove()
        {
            _listenerRegistration.Remove();
        }
    }
}
