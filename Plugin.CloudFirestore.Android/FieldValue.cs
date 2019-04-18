using System;
namespace Plugin.CloudFirestore
{
    public partial class FieldValue
    {
        internal Firebase.Firestore.FieldValue ToNative()
        {
            switch (_fieldValueType)
            {
                case FieldValueType.Delete:
                    return Firebase.Firestore.FieldValue.Delete();
                case FieldValueType.ServerTimestamp:
                    return Firebase.Firestore.FieldValue.ServerTimestamp();
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
