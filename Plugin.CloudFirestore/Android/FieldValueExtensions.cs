using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Android.Runtime;
using Firebase;
using Plugin.CloudFirestore.Attributes;

namespace Plugin.CloudFirestore
{
    internal static class FieldValueExtensions
    {
        public static Java.Lang.Object ToNativeFieldValue(this object fieldValue)
        {
            if (fieldValue == null)
                return null;

            switch (fieldValue)
            {
                case bool @bool:
                    return new Java.Lang.Boolean(@bool);
                case byte @byte:
                    return new Java.Lang.Long(@byte);
                case double @doble:
                    return new Java.Lang.Double(@doble);
                case float @float:
                    return new Java.Lang.Double(@float);
                case int @int:
                    return new Java.Lang.Long(@int);
                case long @long:
                    return new Java.Lang.Long(@long);
                case sbyte @sbyte:
                    return new Java.Lang.Long(@sbyte);
                case short @short:
                    return new Java.Lang.Long(@short);
                case uint @uint:
                    return new Java.Lang.Long(@uint);
                case ulong @ulong:
                    return new Java.Lang.Double(@ulong);
                case ushort @ushort:
                    return new Java.Lang.Long(@ushort);
                case decimal @decimal:
                    return new Java.Lang.Double((double)@decimal);
                case string @string:
                    return new Java.Lang.String(@string);
                case DateTime dateTime:
                    return new Timestamp(dateTime).ToNative();
                case DateTimeOffset dateTimeOffset:
                    return new Timestamp(dateTimeOffset).ToNative();
                case Timestamp timestamp:
                    return timestamp.ToNative();
                case GeoPoint geoPoint:
                    return geoPoint.ToNative();
                case DocumentReferenceWrapper documentReference:
                    return (Firebase.Firestore.DocumentReference)documentReference;
                case byte[] @byte:
                    return Firebase.Firestore.Blob.FromBytes(@byte);
                case Stream stream:
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return Firebase.Firestore.Blob.FromBytes(ms.ToArray());
                    }
                case IList list:
                    {
                        var javaList = new JavaList();
                        foreach (var val in list)
                        {
                            javaList.Add(val.ToNativeFieldValue());
                        }
                        return javaList;
                    }
                case IDictionary dictionary:
                    {
                        var javaDictionary = new JavaDictionary();
                        foreach (var key in dictionary.Keys)
                        {
                            javaDictionary.Add(key.ToString(), dictionary[key].ToNativeFieldValue());
                        }
                        return javaDictionary;
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

                        var javaDictionary = new JavaDictionary();
                        var properties = type.GetProperties();
                        foreach (var property in properties)
                        {
                            var (key, @object) = GetKeyAndObject(fieldValue, property);
                            if (key != null)
                            {
                                javaDictionary.Add(key, @object);
                            }
                        }

                        return javaDictionary;
                    }
            }
        }

        public static JavaDictionary<string, Java.Lang.Object> ToNativeFieldValues(this object fieldValues)
        {
            if (fieldValues == null)
                return null;

            if (fieldValues is IDictionary dictionary)
            {
                var resultDictionary = new JavaDictionary<string, Java.Lang.Object>();
                foreach (var key in dictionary.Keys)
                {
                    resultDictionary.Add(key.ToString(), dictionary[key].ToNativeFieldValue());
                }
                return resultDictionary;
            }

            var properties = fieldValues.GetType().GetProperties();
            var values = new JavaDictionary<string, Java.Lang.Object>();

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

        private static (string Key, Java.Lang.Object Object) GetKeyAndObject(object fieldValue, PropertyInfo property)
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
                    return (key, Firebase.Firestore.FieldValue.ServerTimestamp());
                }

                return (key, value.ToNativeFieldValue());
            }

            return (null, null);
        }

        public static object ToFieldValue(this Java.Lang.Object fieldValue, Type type)
        {
            if (fieldValue == null)
                return null;

            switch (fieldValue)
            {
                case Java.Lang.Boolean @bool:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GenericTypeArguments[0];
                    }
                    return Convert.ChangeType((bool)@bool, type);
                case Java.Lang.Long @long:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GenericTypeArguments[0];
                    }
                    return Convert.ChangeType((long)@long, type);
                case Java.Lang.Double @double:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        type = type.GenericTypeArguments[0];
                    }
                    return Convert.ChangeType((double)@double, type);
                case Java.Lang.String @string:
                    return fieldValue.ToString();
                case Firebase.Timestamp timestamp:
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
                case Java.Util.Date date:
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
                case JavaList javaList:
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

                        foreach (var data in javaList)
                        {
                            var value = data;
                            if (value is Java.Lang.Object javaObject)
                            {
                                value = javaObject.ToFieldValue(genericType);
                            }
                            else if (value != null && genericType != typeof(object))
                            {
                                if (genericType.IsGenericType && genericType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                {
                                    genericType = genericType.GenericTypeArguments[0];
                                }
                                value = Convert.ChangeType(value, genericType);
                            }
                            list.Add(value);
                        }
                        return list;
                    }
				case Java.Util.AbstractList javaList:
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

                        var iterator = javaList.Iterator();
                        while (iterator.HasNext)
						{
							object value = iterator.Next();
							if (value is Java.Lang.Object javaObject)
							{
								value = javaObject.ToFieldValue(genericType);
							}
							else if (value != null && genericType != typeof(object))
							{
								if (genericType.IsGenericType && genericType.GetGenericTypeDefinition() == typeof(Nullable<>))
								{
									genericType = genericType.GenericTypeArguments[0];
								}
								value = Convert.ChangeType(value, genericType);
							}
							list.Add(value);
						}
						return list;
					}
                case JavaDictionary dictionary:
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

                            foreach (var key in dictionary.Keys)
                            {
                                var value = dictionary[key];
                                if (value is Java.Lang.Object javaObject)
                                {
                                    value = javaObject.ToFieldValue(genericType);
                                }
                                else if (value != null && genericType != typeof(object))
                                {
                                    if (genericType.IsGenericType && genericType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        genericType = genericType.GenericTypeArguments[0];
                                    }
                                    value = Convert.ChangeType(value, genericType);
                                }
                                dict.Add(key.ToString(), value);
                            }
                        }
                        else
                        {
                            var properties = type.GetProperties();
                            var mappedProperties = properties.Select(p => (Property: p, Attribute: Attribute.GetCustomAttribute(p, typeof(MapToAttribute)) as MapToAttribute))
                                                             .Where(t => t.Attribute != null)
                                                             .ToDictionary(t => t.Attribute.Mapping, t => t.Property);
                            var igonoredProperties = properties.Where(p => Attribute.GetCustomAttribute(p, typeof(IgnoredAttribute)) != null);

                            foreach (var key in dictionary.Keys)
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
                                    var value = dictionary[key];
                                    if (value is Java.Lang.Object javaObject)
                                    {
                                        value = javaObject.ToFieldValue(property.PropertyType);
                                    }
                                    else if (value != null)
                                    {
                                        var propertyType = property.PropertyType;
                                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                        {
                                            propertyType = propertyType.GenericTypeArguments[0];
                                        }
                                        value = Convert.ChangeType(value, propertyType);
                                    }
                                    property.SetValue(@object, value);
                                }
                            }
                        }
                        return @object;
                    }
                case Java.Util.AbstractMap map:
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

                            foreach (var key in map.KeySet())
                            {
                                object value = map.Get(key.ToString());
                                if (value is Java.Lang.Object javaObject)
                                {
                                    value = javaObject.ToFieldValue(genericType);
                                }
                                else if (value != null && genericType != typeof(object))
                                {
                                    if (genericType.IsGenericType && genericType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                    {
                                        genericType = genericType.GenericTypeArguments[0];
                                    }
                                    value = Convert.ChangeType(value, genericType);
                                }
                                dict.Add(key.ToString(), value);
                            }
                        }
                        else
                        {
                            var properties = type.GetProperties();
                            var mappedProperties = properties.Select(p => (Property: p, Attribute: Attribute.GetCustomAttribute(p, typeof(MapToAttribute)) as MapToAttribute))
                                                             .Where(t => t.Attribute != null)
                                                             .ToDictionary(t => t.Attribute.Mapping, t => t.Property);
                            var igonoredProperties = properties.Where(p => Attribute.GetCustomAttribute(p, typeof(IgnoredAttribute)) != null);

                            foreach (var key in map.KeySet())
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
                                    object value = map.Get(key.ToString());
                                    if (value is Java.Lang.Object javaObject)
                                    {
                                        value = javaObject.ToFieldValue(property.PropertyType);
                                    }
                                    else if (value != null)
                                    {
                                        var propertyType = property.PropertyType;
                                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                                        {
                                            propertyType = propertyType.GenericTypeArguments[0];
                                        }
                                        value = Convert.ChangeType(value, propertyType);
                                    }
                                    property.SetValue(@object, value);
                                }
                            }
                        }
                        return @object;
                    }
                case Firebase.Firestore.Blob blob:
                    if (type == typeof(byte[]))
                    {
                        return blob.ToBytes();
                    }
                    return new MemoryStream(blob.ToBytes());
                case Firebase.Firestore.GeoPoint geoPoint:
                    return new GeoPoint(geoPoint);
                case Firebase.Firestore.DocumentReference documentReference:
                    return new DocumentReferenceWrapper(documentReference);
                default:
                    throw new ArgumentOutOfRangeException($"{fieldValue.GetType().FullName} is not supported");
            }
        }
    }
}
