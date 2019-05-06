using System;

namespace Plugin.CloudFirestore
{
    internal static class SourceExtensions
    {
        public static Firebase.Firestore.Source ToNative(this Source source)
        {
            switch (source)
            {
                case Source.Default:
                    return Firebase.Firestore.Source.Default;
                case Source.Server:
                    return Firebase.Firestore.Source.Server;
                case Source.Cache:
                    return Firebase.Firestore.Source.Cache;
                default:
                    throw new ArgumentOutOfRangeException(nameof(source));
            }
        }
    }
}
