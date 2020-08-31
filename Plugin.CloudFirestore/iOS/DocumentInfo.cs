using System;
using System.Collections.Generic;
using Foundation;

namespace Plugin.CloudFirestore
{
    internal partial class DocumentInfo<T>
    {
        public Dictionary<object, object> ConvertToFieldObject(object target)
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
            if (value is NSArray)
            {
                documentInfo = _listDocumentInfo ?? ObjectProvider.GetDocumentInfo<List<object>>();
            }
            else if (value is NSDictionary && _type == typeof(object))
            {
                documentInfo = ObjectProvider.GetDocumentInfo<Dictionary<string, object>>();
            }
            return documentInfo.Create(value, serverTimestampBehavior);
        }
    }
}
