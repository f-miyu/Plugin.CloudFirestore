using System;
using Firebase.CloudFirestore;

namespace Plugin.CloudFirestore
{
    public partial interface IDocumentReference
    {
        internal DocumentReference ToNative();
    }
}
