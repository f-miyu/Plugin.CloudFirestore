using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Runtime;

namespace Plugin.CloudFirestore
{
    internal static class FieldValueExtensions
    {
        public static Java.Lang.Object ToNativeFieldValue(this object fieldValue, IDocumentFieldInfo fieldInfo = null)
        {
            if (fieldValue == null)
                return null;

            switch (fieldValue)
            {
                case bool @bool:
                    return new Java.Lang.Boolean(@bool);
                case byte @byte:
                    return new Java.Lang.Long(@byte);
                case double @double:
                    return new Java.Lang.Double(@double);
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
                case FieldValue firestoreFieldValue:
                    return firestoreFieldValue.ToNative();
                case FieldPath fieldPath:
                    return fieldPath.ToNative();
                default:
                    {
                        var type = fieldValue.GetType();

                        if (type.IsPrimitive)
                            throw new NotSupportedException($"{type.FullName} is not supported");

                        fieldInfo ??= new DocumentFieldInfo(type);

                        return (Java.Lang.Object)fieldInfo.DocumentInfo.ConvertToFieldValue(fieldValue);
                    }
            }
        }

        public static JavaDictionary<string, Java.Lang.Object> ToNativeFieldValues<T>(this T fieldValues)
        {
            if (fieldValues == null)
                return null;

            return (JavaDictionary<string, Java.Lang.Object>)ObjectProvider.GetDocumentInfo<T>().ConvertToFieldObject(fieldValues);
        }

        public static JavaDictionary<string, Java.Lang.Object> ToNativeFieldValues(this object fieldValues)
        {
            if (fieldValues == null)
                return null;

            return (JavaDictionary<string, Java.Lang.Object>)ObjectProvider.GetDocumentInfo(fieldValues.GetType()).ConvertToFieldObject(fieldValues);
        }

        public static object ToFieldValue(this Java.Lang.Object fieldValue, IDocumentFieldInfo fieldInfo = null)
        {
            if (fieldValue == null)
                return null;

            var type = fieldInfo?.FieldType ?? typeof(object);

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
                        fieldInfo ??= new DocumentFieldInfo<List<object>>();
                        return fieldInfo.DocumentInfo.Create(javaList);
                    }
                case Java.Util.AbstractList javaList:
                    {
                        fieldInfo ??= new DocumentFieldInfo<List<object>>();
                        return fieldInfo.DocumentInfo.Create(javaList);
                    }
                case JavaDictionary dictionary:
                    {
                        fieldInfo ??= new DocumentFieldInfo<Dictionary<string, object>>();
                        return fieldInfo.DocumentInfo.Create(dictionary);
                    }
                case Java.Util.AbstractMap map:
                    {
                        fieldInfo ??= new DocumentFieldInfo<Dictionary<string, object>>();
                        return fieldInfo.DocumentInfo.Create(map);
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
