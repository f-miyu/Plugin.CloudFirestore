using System;
using System.Collections.Generic;
using Firebase.CloudFirestore;
using Foundation;

namespace Plugin.CloudFirestore
{
    internal partial class DictionaryDocumentInfo<T>
    {
        private object PlatformConvertToFieldObject(object target)
        {
            var ret = new Dictionary<object, object>();

            var adapter = GetDictionaryAdapter(target);
            var enumerator = adapter.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Entry;
                ret[entry.Key.ToString()] = entry.Value.ToNativeFieldValue(_documentFieldInfo);
            }

            return ret;
        }

        private object PlatformConvertToFieldValue(object target)
        {
            var ret = new NSMutableDictionary();

            var adapter = GetDictionaryAdapter(target);
            var enumerator = adapter.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Entry;
                ret[new NSString(entry.Key.ToString())] = (NSObject)entry.Value.ToNativeFieldValue(_documentFieldInfo);
            }

            return ret;
        }

        private object PlatformCreate(object value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            return value switch
            {
                DocumentSnapshot snapshot => PlatformCreate(snapshot, serverTimestampBehavior),
                NSDictionary dictionary => PlatformCreate(dictionary),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object PlatformCreate(DocumentSnapshot snapshot, ServerTimestampBehavior? serverTimestampBehavior)
        {
            if (!snapshot.Exists)
            {
                return default;
            }

            NSDictionary<NSString, NSObject> data;
            if (serverTimestampBehavior == null)
            {
                data = snapshot.Data;
            }
            else
            {
                data = snapshot.GetData(serverTimestampBehavior.Value.ToNative());
            }

            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            foreach (var (key, val) in data)
            {
                try
                {
                    object convertedKey = key.ToString();
                    if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                    {
                        convertedKey = Convert.ChangeType(key.ToString(), _dictionaryKeyType);
                    }
                    adapter[convertedKey] = val.ToFieldValue(_documentFieldInfo);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"{key} is invalid: {e.Message}");
                    throw;
                }
            }

            return ret;
        }

        private object PlatformCreate(NSDictionary dictionary)
        {
            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            foreach (var (key, val) in dictionary)
            {
                object convertedKey = key.ToString();
                if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                {
                    convertedKey = Convert.ChangeType(key.ToString(), _dictionaryKeyType);
                }
                adapter[convertedKey] = val.ToFieldValue(_documentFieldInfo);
            }

            return ret;
        }
    }
}
