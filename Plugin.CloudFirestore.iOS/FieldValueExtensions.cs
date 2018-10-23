using System;
using Foundation;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.IO;
using Plugin.CloudFirestore.Attributes;
using System.Reflection;

namespace Plugin.CloudFirestore
{
    public static class FieldValueExtensions
    {
        public static object ToNativeFieldValue<T>(this T fieldValue)
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
                    return NSDate.FromTimeIntervalSinceReferenceDate((dateTime - new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
                case DateTimeOffset dateTimeOffset:
                    return NSDate.FromTimeIntervalSinceReferenceDate((dateTimeOffset - new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
                case GeoPoint geoPoint:
                    return new Firebase.CloudFirestore.GeoPoint(geoPoint.Latitude, geoPoint.Longitude);
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
                default:
                    {
                        var type = fieldValue.GetType();

                        if (type.IsPrimitive)
                            throw new NotSupportedException($"{type.FullName} is not supported");

                        var ndictionary = new NSMutableDictionary();
                        var properties = type.GetProperties();

                        foreach (var property in properties)
                        {
                            var idAttribute = Attribute.GetCustomAttribute(property, typeof(IdAttribute));
                            var igonoredAttribute = Attribute.GetCustomAttribute(property, typeof(IgnoredAttribute));

                            if (idAttribute == null && igonoredAttribute == null)
                            {
                                var mapToAttribute = (MapToAttribute)Attribute.GetCustomAttribute(property, typeof(MapToAttribute));
                                var key = mapToAttribute != null ? new NSString(mapToAttribute.Mapping) : new NSString(property.Name);

                                if (ndictionary.ContainsKey(key))
                                {
                                    throw new ArgumentException($"An item with the same key has already been added. Key: {key}");
                                }
                                ndictionary.Add(key, (NSObject)property.GetValue(fieldValue).ToNativeFieldValue());
                            }
                        }

                        return ndictionary;
                    }
            }
        }

        public static Dictionary<object, object> ToNativeFieldValues<T>(this T fieldValues)
        {
            if (fieldValues == null)
                return null;

            if (fieldValues is IDictionary<string, object> dictionary)
            {
                return dictionary.ToDictionary(pair => (object)pair.Key, pair => pair.Value.ToNativeFieldValue());
            }

            var properties = fieldValues.GetType().GetProperties();
            var values = new Dictionary<object, object>();

            foreach (var property in properties)
            {
                var idAttribute = Attribute.GetCustomAttribute(property, typeof(IdAttribute));
                var igonoredAttribute = Attribute.GetCustomAttribute(property, typeof(IgnoredAttribute));

                if (idAttribute == null && igonoredAttribute == null)
                {
                    var mapToAttribute = (MapToAttribute)Attribute.GetCustomAttribute(property, typeof(MapToAttribute));
                    var key = mapToAttribute != null ? mapToAttribute.Mapping : property.Name;

                    values.Add(key, property.GetValue(fieldValues).ToNativeFieldValue());
                }
            }

            return values;
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
                    return Convert.ChangeType(number.DoubleValue, type);
                case NSString @string:
                    return @string.ToString();
                case NSDate date:
                    {
                        var dateTimeOffset = new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(date.SecondsSinceReferenceDate);
                        if (type == typeof(DateTime) || type == typeof(DateTime?))
                        {
                            return dateTimeOffset.LocalDateTime;
                        }
                        return dateTimeOffset;
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
                    return new GeoPoint(geoPoint.Latitude, geoPoint.Longitude);
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
