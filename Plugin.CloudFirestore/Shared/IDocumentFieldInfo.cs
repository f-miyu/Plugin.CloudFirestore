using System;
namespace Plugin.CloudFirestore
{
    internal interface IDocumentFieldInfo
    {
        Type FieldType { get; }
        IDocumentInfo DocumentInfo { get; }
    }
}
