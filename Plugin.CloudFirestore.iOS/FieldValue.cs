using System;

namespace Plugin.CloudFirestore
{
    public partial class FieldValue
    {
        internal Firebase.CloudFirestore.FieldValue ToNative()
        {
            switch (_fieldValueType)
            {
                case FieldValueType.Delete:
                    return Firebase.CloudFirestore.FieldValue.Delete;
                case FieldValueType.ServerTimestamp:
                    return Firebase.CloudFirestore.FieldValue.ServerTimestamp;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
