using System;

namespace Plugin.CloudFirestore
{
    public partial class FieldValue
    {
        private enum FieldValueType
        {
            Delete,
            ServerTimestamp
        }

        private FieldValueType _fieldValueType;

        private FieldValue(FieldValueType fieldValueType)
        {
            _fieldValueType = fieldValueType;
        }

        public static FieldValue Delete => new FieldValue(FieldValueType.Delete);

        public static FieldValue ServerTimestamp => new FieldValue(FieldValueType.ServerTimestamp);
    }
}
