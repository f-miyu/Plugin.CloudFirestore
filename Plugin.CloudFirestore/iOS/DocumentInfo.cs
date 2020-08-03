using System;
using System.Collections.Generic;
using Foundation;

namespace Plugin.CloudFirestore
{
    internal partial class DocumentInfo<T>
    {
        private object PlatformCreate(object value, ServerTimestampBehavior? serverTimestampBehavior)
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
