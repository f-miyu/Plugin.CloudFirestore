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
        public static object ToNativeFieldValue(this object? fieldValue, IDocumentFieldInfo? fieldInfo = null)
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
                    return new NSNull();
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
                    if (@ulong > long.MaxValue)
                    {
                        throw new OverflowException();
                    }
                    return new NSNumber(@ulong);
                case ushort @ushort:
                    return new NSNumber(@ushort);
                case decimal @decimal:
                    return new NSNumber(decimal.ToDouble(@decimal));
                case char @char:
                    return new NSString(@char.ToString());
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
                case IDocumentReference documentReference:
                    return documentReference.ToNative();
                case byte[] @byte:
                    return NSData.FromArray(@byte);
                case Stream stream:
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        return NSData.FromArray(ms.ToArray());
                    }
                case FieldValue firestoreFieldValue:
                    return firestoreFieldValue.ToNative();
                case FieldPath fieldPath:
                    return fieldPath.ToNative();
                default:
                    fieldInfo ??= new DocumentFieldInfo(fieldValue.GetType());
                    return fieldInfo.DocumentInfo.ConvertToFieldValue(fieldValue);
            }
        }

        public static NSDictionary<TKey, NSObject>? ToNativeFieldValues<T, TKey>(this T fieldValues) where TKey : NSObject
        {
            if (fieldValues is null)
                return null;

            var dic = ObjectProvider.GetDocumentInfo<T>().ConvertToFieldObject(fieldValues);
            if (dic.Count == 0)
            {
                return new NSDictionary<TKey, NSObject>();
            }
            return NSDictionary<TKey, NSObject>.FromObjectsAndKeys(dic.Values.ToArray(), dic.Keys.ToArray(), dic.Count);
        }

        public static NSDictionary<TKey, NSObject>? ToNativeFieldValues<TKey>(this object? fieldValues) where TKey : NSObject
        {
            if (fieldValues is null)
                return null;

            var dic = ObjectProvider.GetDocumentInfo(fieldValues.GetType()).ConvertToFieldObject(fieldValues);
            if (dic.Count == 0)
            {
                return new NSDictionary<TKey, NSObject>();
            }
            return NSDictionary<TKey, NSObject>.FromObjectsAndKeys(dic.Values.ToArray(), dic.Keys.ToArray(), dic.Count);
        }

        public static object? ToFieldValue(this object? fieldValue, IDocumentFieldInfo fieldInfo)
        {
            return (fieldValue switch
            {
                null => new DocumentObject(),
                NSNull _ => new DocumentObject(),
                NSNumber number when number.IsBoolean() => new DocumentObject(number.BoolValue),
                NSNumber number when number.IsInteger() => new DocumentObject(number.LongValue),
                NSNumber number => new DocumentObject(number.DoubleValue),
                NSString @string => new DocumentObject(@string.ToString()),
                string @string => new DocumentObject(@string),
                Firebase.CloudFirestore.Timestamp timestamp => new DocumentObject(new Timestamp(timestamp)),
                NSDate date => new DocumentObject(new Timestamp(date)),
                NSArray array => DocumentObject.CreateAsList((fieldInfo) => fieldInfo.DocumentInfo.Create(array)),
                NSDictionary dictionary => DocumentObject.CreateAsDictionary((fieldInfo) => fieldInfo.DocumentInfo.Create(dictionary)),
                NSData data => new DocumentObject(data.ToArray()),
                Firebase.CloudFirestore.GeoPoint geoPoint => new DocumentObject(new GeoPoint(geoPoint)),
                Firebase.CloudFirestore.DocumentReference documentReference => new DocumentObject(new DocumentReferenceWrapper(documentReference)),
                _ => throw new ArgumentOutOfRangeException(nameof(fieldValue), $"{fieldValue.GetType().FullName} is not supported")
            }).GetFieldValue(fieldInfo);
        }
    }
}
