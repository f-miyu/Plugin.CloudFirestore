using System;
namespace Plugin.CloudFirestore
{
    internal static class SourceExtensions
    {
        public static Firebase.CloudFirestore.FirestoreSource ToNative(this Source source)
        {
            switch (source)
            {
                case Source.Default:
                    return Firebase.CloudFirestore.FirestoreSource.Default;
                case Source.Server:
                    return Firebase.CloudFirestore.FirestoreSource.Server;
                case Source.Cache:
                    return Firebase.CloudFirestore.FirestoreSource.Cache;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }
        }
    }
}
