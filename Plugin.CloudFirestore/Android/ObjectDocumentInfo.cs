using System;
using System.Collections.Generic;
using Android.Runtime;
using Firebase.Firestore;
using Java.Util;

namespace Plugin.CloudFirestore
{
    internal partial class ObjectDocumentInfo<T>
    {
        public JavaDictionary<string, Java.Lang.Object?> ConvertToFieldObject(object target)
        {
            var targetType = target.GetType();
            if (targetType != null && _type != targetType)
            {
                return ObjectProvider.GetDocumentInfo(targetType).ConvertToFieldObject(target);
            }
            return Convert(target);
        }

        public object ConvertToFieldValue(object target)
        {
            var targetType = target.GetType();
            if (targetType != null && _type != targetType)
            {
                return ObjectProvider.GetDocumentInfo(targetType).ConvertToFieldValue(target);
            }
            return Convert(target);
        }

        private JavaDictionary<string, Java.Lang.Object?> Convert(object target)
        {
            var ret = new JavaDictionary<string, Java.Lang.Object?>();

            foreach (var fieldInfo in DocumentFieldInfos.Values)
            {
                if (!fieldInfo.IsId && !fieldInfo.IsIgnored)
                {
                    var value = fieldInfo.GetValue(target);

                    if (fieldInfo.IsServerTimestampValue(value))
                    {
                        ret[fieldInfo.Name] = Firebase.Firestore.FieldValue.ServerTimestamp();
                    }
                    else
                    {
                        ret[fieldInfo.Name] = value.ToNativeFieldValue(fieldInfo);
                    }
                }
            }

            return ret;
        }

        public object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            return value switch
            {
                DocumentSnapshot snapshot => Create(snapshot, serverTimestampBehavior),
                JavaDictionary dictionary => Create(dictionary),
                AbstractMap map => Create(map),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object? Create(DocumentSnapshot snapshot, ServerTimestampBehavior? serverTimestampBehavior)
        {
            if (!snapshot.Exists())
            {
                return default;
            }

            IDictionary<string, Java.Lang.Object> data;
            if (serverTimestampBehavior == null)
            {
                data = snapshot.Data;
            }
            else
            {
                data = snapshot.GetData(serverTimestampBehavior.Value.ToNative());
            }

            var ret = Create();

            foreach (var fieldInfo in DocumentFieldInfos.Values)
            {
                try
                {
                    if (fieldInfo.IsId)
                    {
                        fieldInfo.SetValue(ret, snapshot.Id.ToFieldValue(fieldInfo));
                    }
                    else if (data.TryGetValue(fieldInfo.Name, out var value))
                    {
                        fieldInfo.SetValue(ret, value.ToFieldValue(fieldInfo));
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException($"'{fieldInfo.Name}' value cannot be converted.", e);
                }
            }

            return ret;
        }

        private object Create(JavaDictionary dictionary)
        {
            var ret = Create();

            var enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Entry;
                if (DocumentFieldInfos.TryGetValue(entry.Key.ToString(), out var fieldInfo))
                {
                    object value = entry.Value;
                    fieldInfo.SetValue(ret, value.ToFieldValue(fieldInfo));
                }
            }

            return ret;
        }

        private object Create(AbstractMap map)
        {
            var ret = Create();

            foreach (var key in map.KeySet()!)
            {
                if (DocumentFieldInfos.TryGetValue(key.ToString(), out var fieldInfo))
                {
                    object? value = map.Get(key.ToString());
                    fieldInfo.SetValue(ret, value.ToFieldValue(fieldInfo));
                }
            }
            return ret;
        }
    }
}
