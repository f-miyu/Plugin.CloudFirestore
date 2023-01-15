using System.Collections.Generic;

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
            if (value is object[])
            {
                documentInfo = _listDocumentInfo ?? ObjectProvider.GetDocumentInfo<List<object>>();
            }
            else if (value is Dictionary<string, object> && _type == typeof(object))
            {
                documentInfo = ObjectProvider.GetDocumentInfo<Dictionary<string, object>>();
            }
            return documentInfo.Create(value, serverTimestampBehavior);
        }
    }
}
