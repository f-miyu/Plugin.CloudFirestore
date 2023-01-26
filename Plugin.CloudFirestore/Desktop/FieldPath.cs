namespace Plugin.CloudFirestore
{
    public partial class FieldPath
    {
        internal Google.Cloud.Firestore.FieldPath ToNative()
        {
            return _isDocumentId ? Google.Cloud.Firestore.FieldPath.DocumentId : new Google.Cloud.Firestore.FieldPath(_fieldNames);
        }
    }
}
