using System;
using System.Linq;

namespace Plugin.CloudFirestore
{
    public partial class FieldValue
    {
        internal object ToNative()
        {

            return _fieldValueType switch
            {
                FieldValueType.Delete => Google.Cloud.Firestore.FieldValue.Delete,
                FieldValueType.ServerTimestamp => Google.Cloud.Firestore.FieldValue.ServerTimestamp,
                FieldValueType.ArrayUnion => Google.Cloud.Firestore.FieldValue.ArrayUnion(_elements?.Select(x => x.ToNativeFieldValue()).ToArray()),
                FieldValueType.ArrayRemove => Google.Cloud.Firestore.FieldValue.ArrayRemove(_elements?.Select(x => x.ToNativeFieldValue()).ToArray()),
                FieldValueType.IncrementLong => Google.Cloud.Firestore.FieldValue.Increment(_longValue),
                FieldValueType.IncrementDouble => Google.Cloud.Firestore.FieldValue.Increment(_doubleValue),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
