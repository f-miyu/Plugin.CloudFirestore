using System;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public partial interface IDocumentSnapshot
    {
        internal DocumentSnapshot ToNative();
    }
}
