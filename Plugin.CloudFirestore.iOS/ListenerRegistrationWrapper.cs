using System;
namespace Plugin.CloudFirestore
{
    public class ListenerRegistrationWrapper : IListenerRegistration
    {
        private readonly Firebase.CloudFirestore.IListenerRegistration _listenerRegistration;

        public ListenerRegistrationWrapper(Firebase.CloudFirestore.IListenerRegistration listenerRegistration)
        {
            _listenerRegistration = listenerRegistration;
        }

        public void Remove()
        {
            _listenerRegistration.Remove();
        }
    }
}
