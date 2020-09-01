using System;
using Firebase.Firestore;

namespace Plugin.CloudFirestore
{
    public partial interface IDocumentReference
    {
        internal DocumentReference ToNative();
    }
}
