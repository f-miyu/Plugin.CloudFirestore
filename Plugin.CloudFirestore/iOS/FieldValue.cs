using System;
using System.Linq;
using Foundation;

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
                case FieldValueType.ArrayUnion:
                    return Firebase.CloudFirestore.FieldValue.FromArrayUnion(_elements?.Select(x => x.ToNativeFieldValue() as NSObject).ToArray());
                case FieldValueType.ArrayRemove:
                    return Firebase.CloudFirestore.FieldValue.FromArrayRemove(_elements?.Select(x => x.ToNativeFieldValue() as NSObject).ToArray());
                case FieldValueType.IncrementLong:
                    return Firebase.CloudFirestore.FieldValue.FromIntegerIncrement(_longValue);
                case FieldValueType.IncrementDouble:
                    return Firebase.CloudFirestore.FieldValue.FromDoubleIncrement(_doubleValue);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
