using System;
using System.Linq;
using Foundation;

namespace Plugin.CloudFirestore
{
    public partial class FieldValue
    {
        internal Firebase.CloudFirestore.FieldValue ToNative()
        {
            return _fieldValueType switch
            {
                FieldValueType.Delete => Firebase.CloudFirestore.FieldValue.Delete,
                FieldValueType.ServerTimestamp => Firebase.CloudFirestore.FieldValue.ServerTimestamp,
                FieldValueType.ArrayUnion => Firebase.CloudFirestore.FieldValue.FromArrayUnion(_elements?.Select(x => x.ToNativeFieldValue() as NSObject).ToArray()),
                FieldValueType.ArrayRemove => Firebase.CloudFirestore.FieldValue.FromArrayRemove(_elements?.Select(x => x.ToNativeFieldValue() as NSObject).ToArray()),
                FieldValueType.IncrementLong => Firebase.CloudFirestore.FieldValue.FromIntegerIncrement(_longValue),
                FieldValueType.IncrementDouble => Firebase.CloudFirestore.FieldValue.FromDoubleIncrement(_doubleValue),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
