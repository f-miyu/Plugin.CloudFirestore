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
        public static Java.Lang.Object? ToNativeFieldValue(this object? fieldValue, IDocumentFieldInfo? fieldInfo = null)
        {
            if (fieldValue is { } && fieldInfo is null)
            {
                fieldInfo = new DocumentFieldInfo(fieldValue.GetType());
            }

            if (fieldInfo?.ConvertTo(fieldValue) is (true, var result))
            {
                fieldValue = result;
                fieldInfo = result != null ? new DocumentFieldInfo(result.GetType()) : null;
            }

            switch (fieldValue)
            {
                case null:
                    return null;
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
                    if (@ulong > long.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    return new Java.Lang.Long((long)@ulong);
                case ushort @ushort:
                    return new Java.Lang.Long(@ushort);
                case decimal @decimal:
                    return new Java.Lang.Double((double)@decimal);
                case char @char:
                    return new Java.Lang.String(@char.ToString());
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
                case IDocumentReference documentReference:
                    return documentReference.ToNative();
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
                    fieldInfo ??= new DocumentFieldInfo(fieldValue.GetType());
                    return (Java.Lang.Object)fieldInfo.DocumentInfo.ConvertToFieldValue(fieldValue);
            }
        }

        public static JavaDictionary<string, Java.Lang.Object?>? ToNativeFieldValues<T>(this T fieldValues)
        {
            if (fieldValues is null)
                return null;

            return ObjectProvider.GetDocumentInfo<T>().ConvertToFieldObject(fieldValues);
        }

        public static JavaDictionary<string, Java.Lang.Object?>? ToNativeFieldValues(this object? fieldValues)
        {
            if (fieldValues is null)
                return null;

            return ObjectProvider.GetDocumentInfo(fieldValues.GetType()).ConvertToFieldObject(fieldValues);
        }

        public static object? ToFieldValue(this object? fieldValue, IDocumentFieldInfo fieldInfo)
        {
            return (fieldValue switch
            {
                null => new DocumentObject(),
                Java.Lang.Boolean @bool => new DocumentObject((bool)@bool),
                bool @bool => new DocumentObject(@bool),
                Java.Lang.Long @long => new DocumentObject((long)@long),
                long @long => new DocumentObject(@long),
                Java.Lang.Double @double => new DocumentObject((double)@double),
                double @double => new DocumentObject(@double),
                Java.Lang.String @string => new DocumentObject(@string.ToString()),
                string @string => new DocumentObject(@string),
                Firebase.Timestamp timestamp => new DocumentObject(new Timestamp(timestamp)),
                Java.Util.Date date => new DocumentObject(new Timestamp(date)),
                JavaList javaList => DocumentObject.CreateAsList((fieldInfo) => fieldInfo.DocumentInfo.Create(javaList)),
                Java.Util.AbstractList javaList => DocumentObject.CreateAsList((fieldInfo) => fieldInfo.DocumentInfo.Create(javaList)),
                JavaDictionary dictionary => DocumentObject.CreateAsDictionary((fieldInfo) => fieldInfo.DocumentInfo.Create(dictionary)),
                Java.Util.AbstractMap map => DocumentObject.CreateAsDictionary((fieldInfo) => fieldInfo.DocumentInfo.Create(map)),
                Firebase.Firestore.Blob blob => new DocumentObject(blob.ToBytes()),
                Firebase.Firestore.GeoPoint geoPoint => new DocumentObject(new GeoPoint(geoPoint)),
                Firebase.Firestore.DocumentReference documentReference => new DocumentObject(new DocumentReferenceWrapper(documentReference)),
                _ => throw new ArgumentOutOfRangeException(nameof(fieldValue), $"{fieldValue.GetType().FullName} is not supported")
            }).GetFieldValue(fieldInfo);
        }
    }
}
