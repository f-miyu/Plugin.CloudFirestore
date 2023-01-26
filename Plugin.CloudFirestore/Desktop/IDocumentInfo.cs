using System;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal partial interface IDocumentInfo
    {
        Dictionary<object, object> ConvertToFieldObject(object target);
        object ConvertToFieldValue(object target);
        object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior = null);
    }
}
