using System;

namespace Plugin.CloudFirestore
{
    public partial class FieldValue
    {
        private enum FieldValueType
        {
            Delete,
            ServerTimestamp,
            ArrayUnion,
            ArrayRemove,
        }

        private FieldValueType _fieldValueType;
        private object[] _elements;

        private FieldValue(FieldValueType fieldValueType)
        {
            _fieldValueType = fieldValueType;
        }

        private FieldValue(FieldValueType fieldValueType, object[] elements)
        {
            _fieldValueType = fieldValueType;
            _elements = elements;
        }

        public static FieldValue Delete => new FieldValue(FieldValueType.Delete);

        public static FieldValue ServerTimestamp => new FieldValue(FieldValueType.ServerTimestamp);

        public static FieldValue ArrayUnion(params object[] elements) => new FieldValue(FieldValueType.ArrayUnion, elements);

        public static FieldValue ArrayRemove(params object[] elements) => new FieldValue(FieldValueType.ArrayRemove, elements);
    }
}
