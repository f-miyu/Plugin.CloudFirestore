using System;
namespace Plugin.CloudFirestore
{
    public class ListenerRegistrationWrapper : IListenerRegistration
    {
        public Firebase.CloudFirestore.IListenerRegistration ListenerRegistration { get; }

        public ListenerRegistrationWrapper(Firebase.CloudFirestore.IListenerRegistration listenerRegistration)
        {
            ListenerRegistration = listenerRegistration;
        }

        public void Remove()
        {
            ListenerRegistration.Remove();
        }
    }
}
