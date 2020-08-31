using System;
using System.Collections.Generic;
using Foundation;

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
            var ret = new NSMutableArray();

            var adapter = GetListAdapter(target);
            foreach (var value in adapter)
            {
                ret.Add((NSObject)value.ToNativeFieldValue(_documentFieldInfo));
            }

            return ret;
        }

        public object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            return value switch
            {
                NSArray array => Create(array),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object Create(NSArray array)
        {
            var ret = Create();
            var adapter = GetListAdapter(ret);

            for (nuint i = 0; i < array.Count; i++)
            {
                adapter.Add(array.GetItem<NSObject>(i).ToFieldValue(_documentFieldInfo));
            }

            return _type.IsArray ? adapter.ToArray() : ret;
        }
    }
}
