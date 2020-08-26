using System;
namespace Plugin.CloudFirestore
{
    public class ListenerRegistrationWrapper : IListenerRegistration, IEquatable<ListenerRegistrationWrapper>
    {
        private readonly Firebase.CloudFirestore.IListenerRegistration _listenerRegistration;

        public ListenerRegistrationWrapper(Firebase.CloudFirestore.IListenerRegistration listenerRegistration)
        {
            _listenerRegistration = listenerRegistration ?? throw new ArgumentNullException(nameof(listenerRegistration));
        }

        public void Remove()
        {
            _listenerRegistration.Remove();
        }

        public void Dispose()
        {
            Remove();
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ListenerRegistrationWrapper);
        }

        public bool Equals(ListenerRegistrationWrapper? other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_listenerRegistration, other._listenerRegistration)) return true;
            return _listenerRegistration.Equals(other._listenerRegistration);
        }

        public override int GetHashCode()
        {
            return _listenerRegistration?.GetHashCode() ?? 0;
        }
    }
}
