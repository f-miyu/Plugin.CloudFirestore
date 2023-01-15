using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal partial class ObjectDocumentInfo<T>
    {
        public Dictionary<object, object> ConvertToFieldObject(object target)
        {
            var targetType = target.GetType();
            if (targetType != null && _type != targetType)
            {
                return ObjectProvider.GetDocumentInfo(targetType).ConvertToFieldObject(target);
            }

            var ret = new Dictionary<object, object>();

            foreach (var fieldInfo in DocumentFieldInfos.Values)
            {
                if (!fieldInfo.IsId && !fieldInfo.IsIgnored)
                {
                    var value = fieldInfo.GetValue(target);

                    if (fieldInfo.IsServerTimestampValue(value))
                    {
                        ret[fieldInfo.Name] = Google.Cloud.Firestore.FieldValue.ServerTimestamp;
                    }
                    else
                    {
                        ret[fieldInfo.Name] = value.ToNativeFieldValue(fieldInfo);
                    }
                }
            }

            return ret;
        }

        public object ConvertToFieldValue(object target)
        {
            var targetType = target.GetType();
            if (targetType != null && _type != targetType)
            {
                return ObjectProvider.GetDocumentInfo(targetType).ConvertToFieldValue(target);
            }

            var ret = new Dictionary<string, object>();

            foreach (var fieldInfo in DocumentFieldInfos.Values)
            {
                if (!fieldInfo.IsId && !fieldInfo.IsIgnored)
                {
                    var value = fieldInfo.GetValue(target);
                    var key = fieldInfo.Name;

                    if (fieldInfo.IsServerTimestampValue(value))
                    {
                        ret[key] = Google.Cloud.Firestore.FieldValue.ServerTimestamp;
                    }
                    else
                    {
                        ret[key] = value.ToNativeFieldValue(fieldInfo);
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

        private object Create(Dictionary<string, object> dictionary)
        {
            object ret = Create();

            foreach (var tuple in dictionary)
            {
                if (DocumentFieldInfos.TryGetValue(tuple.Key.ToString(), out var fieldInfo))
                {
                    fieldInfo.SetValue(ret, tuple.Value.ToFieldValue(fieldInfo));
                }
            }

            return ret;
        }
    }
}
