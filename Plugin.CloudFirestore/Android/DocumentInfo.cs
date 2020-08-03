using System;
using System.Collections.Generic;
using Android.Runtime;
using Java.Util;

namespace Plugin.CloudFirestore
{
    internal partial class DocumentInfo<T>
    {
        private object PlatformCreate(object value, ServerTimestampBehavior? serverTimestampBehavior)
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
