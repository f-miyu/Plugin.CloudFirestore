using System;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using Plugin.CloudFirestore.Attributes;
using System.Reflection;
using Firebase.CloudFirestore;

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
                case double @doble:
                    return new NSNumber(@doble);
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
                        var properties = type.GetProperties();

                        foreach (var property in properties)
                        {
                            var (key, @object) = GetKeyAndObject(fieldValue, property);

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

            var properties = fieldValues.GetType().GetProperties();
            var values = new Dictionary<object, object>();

            foreach (var property in properties)
            {
                var (key, @object) = GetKeyAndObject(fieldValues, property);
                if (key != null)
                {
                    values.Add(key, @object);
                }
            }

            return values;
        }

        private static (string Key, object Object) GetKeyAndObject(object fieldValue, PropertyInfo property)
        {
            var idAttribute = Attribute.GetCustomAttribute(property, typeof(IdAttribute));
            var igonoredAttribute = Attribute.GetCustomAttribute(property, typeof(IgnoredAttribute));


            if (idAttribute == null && igonoredAttribute == null)
            {
                var value = property.GetValue(fieldValue);
                var mapToAttribute = (MapToAttribute)Attribute.GetCustomAttribute(property, typeof(MapToAttribute));
                var key = mapToAttribute != null ? mapToAttribute.Mapping : property.Name;

                var serverTimestampAttribute = (Attributes.ServerTimestampAttribute)Attribute.GetCustomAttribute(property, typeof(Attributes.ServerTimestampAttribute));
                if (serverTimestampAttribute != null &&
                    (serverTimestampAttribute.CanReplace || value == null ||
                    (value is DateTime dateTime && dateTime == default) ||
                    (value is DateTimeOffset dateTimeOffset && dateTimeOffset == default) ||
                    (value is Timestamp timestamp && timestamp == default)))
                {
                    return (key, Firebase.CloudFirestore.FieldValue.ServerTimestamp);
                }

                return (key, value.ToNativeFieldValue());
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
                            list = (IList)Activator.CreateInstance(type);
                        }
                        else if (type.IsGenericType)
                        {
                            var listType = typeof(List<>).MakeGenericType(type.GenericTypeArguments[0]);
                            list = (IList)Activator.CreateInstance(listType);
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
                            @object = Activator.CreateInstance(dictionaryType);
                        }
                        else
                        {
                            @object = Activator.CreateInstance(type);
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
                            var properties = type.GetProperties();
                            var mappedProperties = properties.Select(p => (Property: p, Attribute: Attribute.GetCustomAttribute(p, typeof(MapToAttribute)) as MapToAttribute))
                                                             .Where(t => t.Attribute != null)
                                                             .ToDictionary(t => t.Attribute.Mapping, t => t.Property);
                            var igonoredProperties = properties.Where(p => Attribute.GetCustomAttribute(p, typeof(IgnoredAttribute)) != null);

                            foreach (var (key, value) in dictionary)
                            {
                                PropertyInfo property;
                                if (mappedProperties.ContainsKey(key.ToString()))
                                {
                                    property = mappedProperties[key.ToString()];
                                }
                                else
                                {
                                    property = type.GetProperty(key.ToString());
                                }

                                if (property != null && !igonoredProperties.Contains(property))
                                {
                                    property.SetValue(@object, value.ToFieldValue(property.PropertyType));
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
