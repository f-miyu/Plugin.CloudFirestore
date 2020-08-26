using System;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Plugin.CloudFirestore
{
    public partial class FieldValue
    {
        internal Firebase.Firestore.FieldValue ToNative()
        {
            return _fieldValueType switch
            {
                FieldValueType.Delete => Firebase.Firestore.FieldValue.Delete(),
                FieldValueType.ServerTimestamp => Firebase.Firestore.FieldValue.ServerTimestamp(),
                FieldValueType.ArrayUnion => Firebase.Firestore.FieldValue.ArrayUnion(_elements?.Select(x => x.ToNativeFieldValue()).ToArray()),
                FieldValueType.ArrayRemove => Firebase.Firestore.FieldValue.ArrayRemove(_elements?.Select(x => x.ToNativeFieldValue()).ToArray()),
                FieldValueType.IncrementLong => Firebase.Firestore.FieldValue.Increment(_longValue),
                FieldValueType.IncrementDouble => Firebase.Firestore.FieldValue.Increment(_doubleValue),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
