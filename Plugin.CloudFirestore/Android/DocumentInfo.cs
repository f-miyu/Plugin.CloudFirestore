using System;
using System.Collections.Generic;
using Android.Runtime;
using Java.Util;

namespace Plugin.CloudFirestore
{
    internal partial class DocumentInfo<T>
    {
        public JavaDictionary<string, Java.Lang.Object?> ConvertToFieldObject(object target)
        {
            return _objectDocumentInfo.ConvertToFieldObject(target);
        }

        public object ConvertToFieldValue(object target)
        {
            if (_listDocumentInfo != null)
            {
                return _listDocumentInfo.ConvertToFieldValue(target);
            }
            return _objectDocumentInfo.ConvertToFieldValue(target);
        }

        public object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior)
        {
            IDocumentInfo documentInfo = _objectDocumentInfo;
            if (value is JavaList || value is AbstractList)
            {
                documentInfo = _listDocumentInfo ?? ObjectProvider.GetDocumentInfo<List<object>>();
            }
            else if ((value is JavaDictionary || value is AbstractMap) && _type == typeof(object))
            {
                documentInfo = ObjectProvider.GetDocumentInfo<Dictionary<string, object>>();
            }
            return documentInfo.Create(value, serverTimestampBehavior);
        }
    }
}
