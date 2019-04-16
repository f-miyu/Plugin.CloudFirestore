using System;
using Firebase.Firestore;
namespace Plugin.CloudFirestore
{
    public class FieldValueWrapper : IFieldValue
    {
        public object Delete => FieldValue.Delete();

        public object ServerTimestamp => FieldValue.ServerTimestamp();
    }
}
