using Google.Cloud.Firestore;

namespace Plugin.CloudFirestore
{
    public partial interface IDocumentSnapshot
    {
        internal DocumentSnapshot ToNative();
    }
}
