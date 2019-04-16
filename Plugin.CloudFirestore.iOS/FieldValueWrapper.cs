using System;
using Firebase.CloudFirestore;
namespace Plugin.CloudFirestore
{
    public class FieldValueWrapper : IFieldValue
    {
        public object Delete => FieldValue.Delete;

        public object ServerTimestamp => FieldValue.ServerTimestamp;
    }
}
