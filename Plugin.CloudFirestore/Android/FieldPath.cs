using System;
namespace Plugin.CloudFirestore
{
    public partial class FieldPath
    {
        internal Firebase.Firestore.FieldPath ToNative()
        {
            return _isDocumentId ? Firebase.Firestore.FieldPath.DocumentId() : Firebase.Firestore.FieldPath.Of(_fieldNames);
        }
    }
}
