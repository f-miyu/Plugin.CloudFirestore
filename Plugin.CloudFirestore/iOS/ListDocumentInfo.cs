using System;
using Foundation;

namespace Plugin.CloudFirestore
{
    internal partial class ListDocumentInfo<T>
    {
        private object PlatformConvertToFieldValue(object target)
        {
            var ret = new NSMutableArray();

            var adapter = GetListAdapter(target);
            foreach (var value in adapter)
            {
                ret.Add((NSObject)value.ToNativeFieldValue(_documentFieldInfo));
            }

            return ret;
        }

        private object PlatformCreate(object value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            return value switch
            {
                NSArray array => PlatformCreate(array),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object PlatformCreate(NSArray array)
        {
            var ret = Create();
            var adapter = GetListAdapter(ret);

            for (nuint i = 0; i < array.Count; i++)
            {
                adapter.Add(array.GetItem<NSObject>(i).ToFieldValue(_documentFieldInfo));
            }

            return ret;
        }
    }
}
