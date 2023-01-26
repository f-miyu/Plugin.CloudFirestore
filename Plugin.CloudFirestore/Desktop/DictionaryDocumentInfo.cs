using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal partial class DictionaryDocumentInfo<T>
    {
        public Dictionary<object, object> ConvertToFieldObject(object target)
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

        public object ConvertToFieldValue(object target)
        {
            var ret = new Dictionary<string, object>();

            var adapter = GetDictionaryAdapter(target);
            var enumerator = adapter.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var entry = enumerator.Entry;
                ret[entry.Key.ToString()] = entry.Value.ToNativeFieldValue(_documentFieldInfo);
            }

            return ret;
        }

        public object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            return value switch
            {
                DocumentSnapshot snapshot => Create(snapshot, serverTimestampBehavior),
                Dictionary<string, object> dictionary => Create(dictionary),
                null => default,
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        private object? Create(DocumentSnapshot snapshot, ServerTimestampBehavior? serverTimestampBehavior)
        {
            if (!snapshot.Exists)
            {
                return default;
            }

            Dictionary<string, object> data;
            if (serverTimestampBehavior == null)
            {
                data = snapshot.ToDictionary();
            }
            else
            {
                throw new NotImplementedException();
                //data = snapshot.GetData(serverTimestampBehavior.Value.ToNative())!;
            }

            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            foreach (var keyValuePair in data)
            {
                try
                {
                    object convertedKey = keyValuePair.Key.ToString();
                    if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                    {
                        convertedKey = Convert.ChangeType(keyValuePair.Key.ToString(), _dictionaryKeyType);
                    }
                    adapter[convertedKey] = keyValuePair.Value.ToFieldValue(_documentFieldInfo);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException($"'{keyValuePair.Key}' value cannot be converted.", e);
                }
            }

            return ret;
        }

        private object Create(Dictionary<string, object> dictionary)
        {
            var ret = Create();
            var adapter = GetDictionaryAdapter(ret);

            foreach (var keyValuePair in dictionary)
            {
                object convertedKey = keyValuePair.Key.ToString();
                if (_dictionaryKeyType != typeof(string) && _dictionaryKeyType != typeof(object))
                {
                    convertedKey = Convert.ChangeType(keyValuePair.Key.ToString(), _dictionaryKeyType);
                }
                adapter[convertedKey] = keyValuePair.Value.ToFieldValue(_documentFieldInfo);
            }

            return ret;
        }
    }
}
