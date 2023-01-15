using AsyncAwaitBestPractices;
using Google.Cloud.Firestore;
using System;

namespace Plugin.CloudFirestore
{
    public class ListenerRegistrationWrapper : IListenerRegistration, IEquatable<ListenerRegistrationWrapper>
    {
        private readonly FirestoreChangeListener _listenerRegistration;

        public ListenerRegistrationWrapper(FirestoreChangeListener listenerRegistration)
        {
            _listenerRegistration = listenerRegistration ?? throw new ArgumentNullException(nameof(listenerRegistration));
        }

        public void Remove()
        {
            _listenerRegistration.StopAsync().SafeFireAndForget();
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
