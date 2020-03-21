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
            IncrementLong,
            IncrementDouble
        }

        private FieldValueType _fieldValueType;
        private object[] _elements;
        private long _longValue;
        private double _doubleValue;

        private FieldValue(FieldValueType fieldValueType)
        {
            _fieldValueType = fieldValueType;
        }

        private FieldValue(FieldValueType fieldValueType, object[] elements)
        {
            _fieldValueType = fieldValueType;
            _elements = elements;
        }

        private FieldValue(FieldValueType fieldValueType, long longValue)
        {
            _fieldValueType = fieldValueType;
            _longValue = longValue;
        }

        private FieldValue(FieldValueType fieldValueType, double doubleValue)
        {
            _fieldValueType = fieldValueType;
            _doubleValue = doubleValue;
        }

        public static FieldValue Delete => new FieldValue(FieldValueType.Delete);

        public static FieldValue ServerTimestamp => new FieldValue(FieldValueType.ServerTimestamp);

        public static FieldValue ArrayUnion(params object[] elements) => new FieldValue(FieldValueType.ArrayUnion, elements);

        public static FieldValue ArrayRemove(params object[] elements) => new FieldValue(FieldValueType.ArrayRemove, elements);

        public static FieldValue Increment(long l) => new FieldValue(FieldValueType.IncrementLong, l);

        public static FieldValue Increment(double l) => new FieldValue(FieldValueType.IncrementDouble, l);
    }
}
