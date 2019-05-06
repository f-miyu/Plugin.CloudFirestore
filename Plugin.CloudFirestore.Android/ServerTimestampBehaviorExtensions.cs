using System;
using Firebase.Firestore;
namespace Plugin.CloudFirestore
{
    internal static class ServerTimestampBehaviorExtensions
    {
        public static DocumentSnapshot.ServerTimestampBehavior ToNative(this ServerTimestampBehavior serverTimestampBehavior)
        {
            switch (serverTimestampBehavior)
            {
                case ServerTimestampBehavior.None:
                    return DocumentSnapshot.ServerTimestampBehavior.None;
                case ServerTimestampBehavior.Estimate:
                    return DocumentSnapshot.ServerTimestampBehavior.Estimate;
                case ServerTimestampBehavior.Previous:
                    return DocumentSnapshot.ServerTimestampBehavior.Previous;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serverTimestampBehavior));
            }
        }
    }
}
