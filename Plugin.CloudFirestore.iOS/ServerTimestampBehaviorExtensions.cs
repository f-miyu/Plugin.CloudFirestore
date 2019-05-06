using System;
namespace Plugin.CloudFirestore
{
    internal static class ServerTimestampBehaviorExtensions
    {
        public static Firebase.CloudFirestore.ServerTimestampBehavior ToNative(this ServerTimestampBehavior serverTimestampBehavior)
        {
            switch (serverTimestampBehavior)
            {
                case ServerTimestampBehavior.None:
                    return Firebase.CloudFirestore.ServerTimestampBehavior.None;
                case ServerTimestampBehavior.Estimate:
                    return Firebase.CloudFirestore.ServerTimestampBehavior.Estimate;
                case ServerTimestampBehavior.Previous:
                    return Firebase.CloudFirestore.ServerTimestampBehavior.Previous;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serverTimestampBehavior));
            }
        }
    }
}
