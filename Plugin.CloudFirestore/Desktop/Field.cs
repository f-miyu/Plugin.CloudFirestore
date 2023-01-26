using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal static class Field
    {
        public static Dictionary<string, object> CreateFields(string field, object? value,
            params object?[] moreFieldsAndValues)
        {
            return CreateFields(new FieldPath(field), value, moreFieldsAndValues);
        }

        public static Dictionary<string, object> CreateFields(FieldPath field, object? value, params object?[] moreFieldsAndValues)
        {
            if (field == null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (moreFieldsAndValues == null)
            {
                throw new ArgumentNullException(nameof(moreFieldsAndValues));
            }

            if (moreFieldsAndValues.Length % 2 != 0)
            {
                throw new ArgumentException("There must be an even number of arguments that alternate between field names and values", nameof(moreFieldsAndValues));
            }

            var fields = new Dictionary<string, object>()
            {
                [field.ToString()] = value
            };

            for (var i = 0; i < moreFieldsAndValues.Length; i += 2)
            {
                fields[moreFieldsAndValues[i].ToString()] = moreFieldsAndValues[i + 1];
            }

            return fields;
        }
    }
}
