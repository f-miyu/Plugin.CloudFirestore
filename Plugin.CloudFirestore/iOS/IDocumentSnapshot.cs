using System;
using Firebase.CloudFirestore;

namespace Plugin.CloudFirestore
{
    public partial interface IDocumentSnapshot
    {
        internal DocumentSnapshot ToNative();
    }
}
