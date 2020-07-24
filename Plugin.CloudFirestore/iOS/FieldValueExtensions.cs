using System;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;

namespace Plugin.CloudFirestore
{
    internal static class FieldValueExtensions
    {
        public static object ToNativeFieldValue(this object fieldValue)
        {
            if (fieldValue == null)
                return new NSNull();

            switch (fieldValue)
            {
                case bool @bool:
                    return new NSNumber(@bool);
                case byte @byte:
                    return new NSNumber(@byte);
                case double @double:
                    return new NSNumber(@double);
                case float @float:
                    return new NSNumber(@float);
                case int @int:
                    return new NSNumber(@int);
                case long @long:
                    return new NSNumber(@long);
                case sbyte @sbyte:
                    return new NSNumber(@sbyte);
                case short @short:
                    return new NSNumber(@short);
                case uint @uint:
                    return new NSNumber(@uint);
                case ulong @ulong:
                    return new NSNumber(@ulong);
                case ushort @ushort:
                    return new NSNumber(@ushort);
                case decimal @decimal:
                    return new NSNumber((double)@decimal);
                case string @string:
                    return new NSString(@string);
                case DateTime dateTime:
                    return new Timestamp(dateTime).ToNative();
                case DateTimeOffset dateTimeOffset:
                    return new Timestamp(dateTimeOffset).ToNative();
                case Timestamp timestamp:
                    return timestamp.ToNative();
                case GeoPoint geoPoint:
                    return geoPoint.ToNative();
                case DocumentReferenceWrapper documentReference:
                    return (Firebase.CloudFirestore.DocumentReference)documentReference;
                case byte[] @byte:
                    return NSData.FromArray(@byte);
                case Stream stream:
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return NSData.FromArray(ms.ToArray());
                    }
                case IList list:
                    {
                        var array = new NSMutableArray();
                        foreach (var val in list)
                        {
                            array.Add((NSObject)val.ToNativeFieldValue());
                        }
                        return array;
                    }
                case IDictionary dictionary:
                    {
                        var ndictionary = new NSMutableDictionary();
                        foreach (var key in dictionary.Keys)
                        {
                            ndictionary.Add(new NSString(key.ToString()), (NSObject)dictionary[key].ToNativeFieldValue());
                        }
                        return ndictionary;
                    }
                case FieldValue firestoreFieldValue:
                    return firestoreFieldValue.ToNative();
                case FieldPath fieldPath:
                    return fieldPath.ToNative();
                default:
                    {
                        var type = fieldValue.GetType();

                        if (type.IsPrimitive)
                            throw new NotSupportedException($"{type.FullName} is not supported");

                        var ndictionary = new NSMutableDictionary();
                        var fieldInfos = DocumentInfoProvider.GetDocumentInfo(type).DocumentFieldInfos.Values;

                        foreach (var fieldInfo in fieldInfos)
                        {
                            var (key, @object) = GetKeyAndObject(fieldValue, fieldInfo);

                            if (key != null)
                            {
                                var keyString = new NSString(key);
                                if (ndictionary.ContainsKey(keyString))
                                {
                                    throw new ArgumentException($"An item with the same key has already been added. Key: {keyString}");
                                }
                                ndictionary.Add(keyString, (NSObject)@object);
                            }
                        }

                        return ndictionary;
                    }
            }
        }

        public static Dictionary<object, object> ToNativeFieldValues(this object fieldValues)
        {
            if (fieldValues == null)
                return null;

            if (fieldValues is IDictionary dictionary)
            {
                var resultDictionary = new Dictionary<object, object>();
                foreach (var key in dictionary.Keys)
                {
                    resultDictionary.Add(key.ToString(), dictionary[key].ToNativeFieldValue());
                }
                return resultDictionary;
            }

            var fieldInfos = DocumentInfoProvider.GetDocumentInfo(fieldValues.GetType()).DocumentFieldInfos.Values;
            var values = new Dictionary<object, object>();

            foreach (var fieldInfo in fieldInfos)
            {
                var (key, @object) = GetKeyAndObject(fieldValues, fieldInfo);
                if (key != null)
                {
                    values.Add(key, @object);
                }
            }

            return values;
        }

        private static (string Key, object Object) GetKeyAndObject(object fieldValue, DocumentFieldInfo fieldInfo)
        {
            if (!fieldInfo.IsId && !fieldInfo.IsIgnored)
            {
                var value = fieldInfo.GetValue(fieldValue);

                if (fieldInfo.IsServerTimestamp &&
                    (fieldInfo.CanReplaceServerTimestamp || value == null ||
                    (value is DateTime dateTime && dateTime == default) ||
                    (value is DateTimeOffset dateTimeOffset && dateTimeOffset == default) ||
                    (value is Timestamp timestamp && timestamp == default)))
                {
                    return (fieldInfo.Name, Firebase.CloudFirestore.FieldValue.ServerTimestamp);
                }

                return (fieldInfo.Name, value.ToNativeFieldValue());
            }

            return (null, null);
        }

        public static object ToFieldValue(this NSObject fieldValue, Type type)
        {
            if (fieldValue == null)
                return null;

            switch (fieldValue)
            {
                case NSNumber number:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GenericTypeArguments[0];
                    }

                    if (type == typeof(bool))
                    {
                        return number.BoolValue;
                    }
                    if (type == typeof(byte))
                    {
                        return number.ByteValue;
                    }
                    if (type == typeof(double))
                    {
                        return number.DoubleValue;
                    }
                    if (type == typeof(float))
                    {
                        return number.FloatValue;
                    }
                    if (type == typeof(int))
                    {
                        return number.Int32Value;
                    }
                    if (type == typeof(long))
                    {
                        return number.Int64Value;
                    }
                    if (type == typeof(sbyte))
                    {
                        return number.SByteValue;
                    }
                    if (type == typeof(short))
                    {
                        return number.Int16Value;
                    }
                    if (type == typeof(uint))
                    {
                        return number.UInt32Value;
                    }
                    if (type == typeof(ulong))
                    {
                        return number.UInt64Value;
                    }
                    if (type == typeof(ushort))
                    {
                        return number.UInt16Value;
                    }
                    return Convert.ChangeType(number.DoubleValue, type);
                case NSString @string:
                    return @string.ToString();
                case Firebase.CloudFirestore.Timestamp timestamp:
                    {
                        var time = new Timestamp(timestamp);
                        if (type == typeof(DateTime) || type == typeof(DateTime?))
                        {
                            return time.ToDateTime();
                        }
                        if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
                        {
                            return time.ToDateTimeOffset();
                        }
                        return time;
                    }
                case NSDate date:
                    {
                        var time = new Timestamp(date);
                        if (type == typeof(DateTime) || type == typeof(DateTime?))
                        {
                            return time.ToDateTime();
                        }
                        if (type == typeof(DateTimeOffset) || type == typeof(DateTimeOffset?))
                        {
                            return time.ToDateTimeOffset();
                        }
                        return time;
                    }
                case NSArray array:
                    {
                        IList list;
                        if (type.GetInterfaces().Contains(typeof(IList)))
                        {
                            list = (IList)CreatorProvider.GetCreator(type).Invoke();
                        }
                        else if (type.IsGenericType)
                        {
                            var listType = typeof(List<>).MakeGenericType(type.GenericTypeArguments[0]);
                            list = (IList)CreatorProvider.GetCreator(listType).Invoke();
                        }
                        else
                        {
                            list = new List<object>();
                        }

                        var genericType = typeof(object);
                        if (type.IsGenericType)
                        {
                            genericType = type.GenericTypeArguments[0];
                        }

                        for (nuint i = 0; i < array.Count; i++)
                        {
                            list.Add(array.GetItem<NSObject>(i).ToFieldValue(genericType));
                        }
                        return list;
                    }
                case NSDictionary dictionary:
                    {
                        object @object;
                        if (type == typeof(object))
                        {
                            @object = new Dictionary<string, object>();
                        }
                        else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                        {
                            var dictionaryType = typeof(Dictionary<,>).MakeGenericType(type.GenericTypeArguments[0], type.GenericTypeArguments[1]);
                            @object = CreatorProvider.GetCreator(dictionaryType).Invoke();
                        }
                        else
                        {
                            @object = CreatorProvider.GetCreator(type).Invoke();
                        }

                        if (@object is IDictionary dict)
                        {
                            var genericType = typeof(object);
                            if (type.IsGenericType)
                            {
                                genericType = type.GenericTypeArguments[1];
                            }

                            foreach (var (key, value) in dictionary)
                            {
                                dict.Add(key.ToString(), value.ToFieldValue(genericType));
                            }
                        }
                        else
                        {
                            var fieldInfos = DocumentInfoProvider.GetDocumentInfo(type).DocumentFieldInfos;
                            foreach (var (key, value) in dictionary)
                            {
                                if (fieldInfos.TryGetValue(key.ToString(), out var fieldInfo))
                                {
                                    fieldInfo.SetValue(@object, value.ToFieldValue(fieldInfo.FieldType));
                                }
                            }
                        }
                        return @object;
                    }
                case NSData data:
                    if (type == typeof(byte[]))
                    {
                        return data.ToArray();
                    }
                    return new MemoryStream(data.ToArray());
                case Firebase.CloudFirestore.GeoPoint geoPoint:
                    return new GeoPoint(geoPoint);
                case Firebase.CloudFirestore.DocumentReference documentReference:
                    return new DocumentReferenceWrapper(documentReference);
                case NSNull @null:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException($"{fieldValue.GetType().FullName} is not supported");
            }
        }

    }
}
