using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal interface IDocumentInfo
    {
        object ConvertToFieldObject(object target);
        object ConvertToFieldValue(object target);
        object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior = null);
        string GetMappingName(string name);
    }
}
