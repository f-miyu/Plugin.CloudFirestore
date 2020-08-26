using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal static class Field
    {
        public static Dictionary<object, object> CreateFields(object field, object? value, params object?[] moreFieldsAndValues)
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

            var fields = new Dictionary<object, object>()
            {
                [field.ToNativeFieldValue()] = value.ToNativeFieldValue()
            };

            for (var i = 0; i < moreFieldsAndValues.Length; i += 2)
            {
                fields[moreFieldsAndValues[i].ToNativeFieldValue()] = moreFieldsAndValues[i + 1].ToNativeFieldValue();
            }

            return fields;
        }
    }
}
