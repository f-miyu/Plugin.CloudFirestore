using System;
using Android.Runtime;
using Java.Util;

namespace Plugin.CloudFirestore
{
    internal partial class ListDocumentInfo<T>
    {
        public JavaDictionary<string, Java.Lang.Object?> ConvertToFieldObject(object target)
        {
            throw new NotImplementedException();
        }

        public object ConvertToFieldValue(object target)
        {
            var ret = new JavaList<Java.Lang.Object?>();

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
                JavaList list => Create(list),
                AbstractList list => Create(list),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object Create(JavaList list)
        {
            var ret = Create();
            var adapter = GetListAdapter(ret);

            foreach (var val in list)
            {
                adapter.Add(val.ToFieldValue(_documentFieldInfo));
            }

            return _type.IsArray ? adapter.ToArray() : ret;
        }

        private object Create(AbstractList list)
        {
            var ret = Create();
            var adapter = GetListAdapter(ret);

            var iterator = list.Iterator()!;
            while (iterator.HasNext)
            {
                object? value = iterator.Next();
                adapter.Add(value.ToFieldValue(_documentFieldInfo));
            }

            return _type.IsArray ? adapter.ToArray() : ret;
        }
    }
}
