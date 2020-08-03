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
        private object PlatformConvertToFieldObject(object target)
        {
            var ret = new JavaDictionary<string, Java.Lang.Object>();

            var adapter = GetDictionaryAdapter(target);
            var enumerator = adapter.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Entry;
                ret.Add(entry.Key.ToString(), entry.Value.ToNativeFieldValue(_documentFieldInfo));
            }

            return ret;
        }

        private object PlatformConvertToFieldValue(object target)
        {
            return PlatformConvertToFieldObject(target);
        }

        private object PlatformCreate(object value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            return value switch
            {
                DocumentSnapshot snapshot => PlatformCreate(snapshot, serverTimestampBehavior),
                JavaDictionary dictionary => PlatformCreate(dictionary),
                AbstractMap map => PlatformCreate(map),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object PlatformCreate(DocumentSnapshot sanpshot, ServerTimestampBehavior? serverTimestampBehavior)
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
                    System.Diagnostics.Debug.WriteLine($"{key} is invalid: {e.Message}");
                    throw;
                }
            }

            return ret;
        }

        private object PlatformCreate(JavaDictionary dictionary)
        {
            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            var enumerator = dictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Entry;
                object value = entry.Value;
                if (value is Java.Lang.Object javaObject)
                {
                    value = javaObject.ToFieldValue(_documentFieldInfo);
                }
                else if (value != null && _dictionaryValueType != typeof(object))
                {
                    value = Convert.ChangeType(value, _dictionaryValueType);
                }

                object key = entry.Key.ToString();
                if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                {
                    key = Convert.ChangeType(key, _dictionaryKeyType);
                }

                adapter[key] = value;
            }

            return ret;
        }

        private object PlatformCreate(AbstractMap map)
        {
            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            foreach (var key in map.KeySet())
            {
                var keyStr = key.ToString();
                object value = map.Get(keyStr);
                if (value is Java.Lang.Object javaObject)
                {
                    value = javaObject.ToFieldValue(_documentFieldInfo);
                }
                else if (value != null && _dictionaryValueType != typeof(object))
                {
                    value = Convert.ChangeType(value, _dictionaryValueType);
                }

                object convertedKey = keyStr;
                if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                {
                    convertedKey = Convert.ChangeType(key, _dictionaryKeyType);
                }

                adapter[convertedKey] = value;
            }

            return ret;
        }
    }
}
