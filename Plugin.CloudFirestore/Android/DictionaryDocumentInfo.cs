using System;
using System.Collections;
using System.Collections.Generic;
using Android.Runtime;
using Firebase.Firestore;
using Java.Util;

namespace Plugin.CloudFirestore
{
    internal partial class DictionaryDocumentInfo<T>
    {
        public JavaDictionary<string, Java.Lang.Object?> ConvertToFieldObject(object target)
        {
            var ret = new JavaDictionary<string, Java.Lang.Object?>();

            var adapter = GetDictionaryAdapter(target);
            var enumerator = adapter.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Entry;
                ret.Add(entry.Key.ToString(), entry.Value.ToNativeFieldValue(_documentFieldInfo));
            }

            return ret;
        }

        public object ConvertToFieldValue(object target)
        {
            return ConvertToFieldObject(target);
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

        private object? Create(DocumentSnapshot sanpshot, ServerTimestampBehavior? serverTimestampBehavior)
        {
            if (!sanpshot.Exists())
            {
                return default;
            }

            IDictionary<string, Java.Lang.Object> data;
            if (serverTimestampBehavior == null)
            {
                data = sanpshot.Data;
            }
            else
            {
                data = sanpshot.GetData(serverTimestampBehavior.Value.ToNative());
            }

            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            foreach (var (key, value) in data)
            {
                try
                {
                    object convertedKey = key;
                    if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                    {
                        convertedKey = Convert.ChangeType(key, _dictionaryKeyType);
                    }

                    adapter[convertedKey] = value.ToFieldValue(_documentFieldInfo);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException($"'{key}' value cannot be converted.", e);
                }
            }

            return ret;
        }

        private object Create(JavaDictionary dictionary)
        {
            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            var enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Entry;
                object value = entry.Value;

                object key = entry.Key.ToString();
                if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                {
                    key = Convert.ChangeType(key, _dictionaryKeyType);
                }

                adapter[key] = value.ToFieldValue(_documentFieldInfo);
            }

            return ret;
        }

        private object Create(AbstractMap map)
        {
            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            foreach (var key in map.KeySet()!)
            {
                var keyStr = key.ToString();
                object? value = map.Get(keyStr);

                object convertedKey = keyStr;
                if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                {
                    convertedKey = Convert.ChangeType(key, _dictionaryKeyType);
                }

                adapter[convertedKey] = value.ToFieldValue(_documentFieldInfo);
            }

            return ret;
        }
    }
}
