using System;
using Android.Runtime;

namespace Plugin.CloudFirestore
{
    internal partial interface IDocumentInfo
    {
        JavaDictionary<string, Java.Lang.Object?> ConvertToFieldObject(object target);
        object ConvertToFieldValue(object target);
        object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior = null);
    }
}
