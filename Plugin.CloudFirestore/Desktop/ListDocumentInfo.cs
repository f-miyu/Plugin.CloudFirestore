using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal partial class ListDocumentInfo<T>
    {
        public Dictionary<object, object> ConvertToFieldObject(object target)
        {
            throw new NotImplementedException();
        }

        public object ConvertToFieldValue(object target)
        {
            var ret = new List<object>();

            var adapter = GetListAdapter(target);
            foreach (var value in adapter)
            {
                ret.Add(value.ToNativeFieldValue(_documentFieldInfo));
            }

            return ret;
        }

        public object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            return value switch
            {
                object[] array => Create(array),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object Create(object[] array)
        {
            var ret = Create();
            var adapter = GetListAdapter(ret);

            for (int i = 0; i < array.Length; i++)
            {
                adapter.Add(array[i].ToFieldValue(_documentFieldInfo));
            }

            return _type.IsArray ? adapter.ToArray() : ret;
        }
    }
}
