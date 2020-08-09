using System;
using System.Collections.Generic;
using Firebase.CloudFirestore;
using Foundation;

namespace Plugin.CloudFirestore
{
    internal partial class ObjectDocumentInfo<T>
    {
        private object PlatformConvertToFieldObject(object target)
        {
            var ret = new Dictionary<object, object>();

            foreach (var fieldInfo in DocumentFieldInfos.Values)
            {
                if (!fieldInfo.IsId && !fieldInfo.IsIgnored)
                {
                    var value = fieldInfo.GetValue(target);

                    if (fieldInfo.IsServerTimestampValue(value))
                    {
                        ret[fieldInfo.Name] = Firebase.CloudFirestore.FieldValue.ServerTimestamp;
                    }
                    else
                    {
                        ret[fieldInfo.Name] = value.ToNativeFieldValue(fieldInfo);
                    }
                }
            }

            return ret;
        }

        private object PlatformConvertToFieldValue(object target)
        {
            var ret = new NSMutableDictionary();

            foreach (var fieldInfo in DocumentFieldInfos.Values)
            {
                if (!fieldInfo.IsId && !fieldInfo.IsIgnored)
                {
                    var value = fieldInfo.GetValue(target);
                    var key = new NSString(fieldInfo.Name);

                    if (fieldInfo.IsServerTimestampValue(value))
                    {
                        ret[key] = Firebase.CloudFirestore.FieldValue.ServerTimestamp;
                    }
                    else
                    {
                        ret[key] = (NSObject)value.ToNativeFieldValue(fieldInfo);
                    }
                }
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

            foreach (var fieldInfo in DocumentFieldInfos.Values)
            {
                try
                {
                    if (fieldInfo.IsId)
                    {
                        fieldInfo.SetValue(ret, snapshot.Id.ToFieldValue(fieldInfo));
                    }
                    else if (data.TryGetValue(new NSString(fieldInfo.Name), out var value))
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

        private object PlatformCreate(NSDictionary dictionary)
        {
            var ret = Create();

            foreach (var (key, val) in dictionary)
            {
                if (DocumentFieldInfos.TryGetValue(key.ToString(), out var fieldInfo))
                {
                    fieldInfo.SetValue(ret, val.ToFieldValue(fieldInfo));
                }
            }

            return ret;
        }
    }
}
