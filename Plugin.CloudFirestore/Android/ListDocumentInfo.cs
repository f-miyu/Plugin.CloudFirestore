using System;
using Android.Runtime;
using Java.Util;

namespace Plugin.CloudFirestore
{
    internal partial class ListDocumentInfo<T>
    {
        private object PlatformConvertToFieldValue(object target)
        {
            var ret = new JavaList<Java.Lang.Object>();

            var adapter = GetListAdapter(target);
            foreach (var value in adapter)
            {
                ret.Add(value.ToNativeFieldValue(_documentFieldInfo));
            }

            return ret;
        }

        private object PlatformCreate(object value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            return value switch
            {
                JavaList list => PlatformCreate(list),
                AbstractList list => PlatformCreate(list),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object PlatformCreate(JavaList list)
        {
            var ret = Create();
            var adapter = GetListAdapter(ret);

            foreach (var val in list)
            {
                object value = val;
                if (value is Java.Lang.Object javaObject)
                {
                    value = javaObject.ToFieldValue(_documentFieldInfo);
                }
                else if (value != null && _listValueType != typeof(object))
                {
                    value = Convert.ChangeType(value, _listValueType);
                }

                adapter.Add(value);
            }

            return ret;
        }

        private object PlatformCreate(AbstractList list)
        {
            var ret = Create();
            var adapter = GetListAdapter(ret);

            var iterator = list.Iterator();
            while (iterator.HasNext)
            {
                object value = iterator.Next();
                if (value is Java.Lang.Object javaObject)
                {
                    value = javaObject.ToFieldValue(_documentFieldInfo);
                }
                else if (value != null && _listValueType != typeof(object))
                {
                    value = Convert.ChangeType(value, _listValueType);
                }

                adapter.Add(value);
            }

            return ret;
        }
    }
}
