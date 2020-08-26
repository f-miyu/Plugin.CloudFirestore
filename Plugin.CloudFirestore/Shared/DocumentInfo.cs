using System;

namespace Plugin.CloudFirestore
{
    internal partial class DocumentInfo<T> : IDocumentInfo
    {
        private readonly IDocumentInfo _objectDocumentInfo;
        private readonly IDocumentInfo? _listDocumentInfo;
        private readonly Type _type = typeof(T);

        public DocumentInfo()
        {
            if (_type.IsDictionaryType())
            {
                _objectDocumentInfo = new DictionaryDocumentInfo<T>();
            }
            else
            {
                _objectDocumentInfo = new ObjectDocumentInfo<T>();
            }

            if (_type.IsListType())
            {
                _listDocumentInfo = new ListDocumentInfo<T>();
            }
        }

        public object ConvertToFieldObject(object target)
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
#if NETSTANDARD
            throw new NotImplementedException();
#else
            return PlatformCreate(value, serverTimestampBehavior);
#endif
        }

        public string GetMappingName(string name)
        {
            return _objectDocumentInfo.GetMappingName(name);
        }
    }
}
