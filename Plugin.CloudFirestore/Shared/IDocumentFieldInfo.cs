using System;
using Plugin.CloudFirestore.Converters;

namespace Plugin.CloudFirestore
{
    internal interface IDocumentFieldInfo
    {
        Type Type { get; }
        Type NullableUnderlyingType { get; }
        IDocumentInfo DocumentInfo { get; }
        (bool IsConverted, object? Result) ConvertTo(object? value);
        (bool IsConverted, object? Result) ConvertFrom(DocumentObject value);
    }
}
