using Google.Cloud.Firestore;

namespace Plugin.CloudFirestore
{
    public partial interface IDocumentReference
    {
        internal DocumentReference ToNative();
    }
}
