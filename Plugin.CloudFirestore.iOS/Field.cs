using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    public static class Field
    {
        public static Dictionary<object, object> CreateFields(object field, object value, params object[] moreFieldsAndValues)
        {
            if (moreFieldsAndValues.Length % 2 != 0)
            {
                throw new ArgumentException(nameof(moreFieldsAndValues));
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
