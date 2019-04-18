using System;
namespace Plugin.CloudFirestore
{
    public partial class FieldPath
    {
        internal Firebase.CloudFirestore.FieldPath ToNative()
        {
            return _isDocumentId ? Firebase.CloudFirestore.FieldPath.GetDocumentId() : new Firebase.CloudFirestore.FieldPath(_fieldNames);
        }
    }
}
