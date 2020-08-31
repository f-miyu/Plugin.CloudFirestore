using System;
using System.Collections;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal partial class ListDocumentInfo<T> : IDocumentInfo
    {
        private enum CreatorType
        {
            SpecifiedType,
            SpecifiedTypeList,
            ObjectList
        };

        private enum ListAdapterFactoryType
        {
            Generic,
            NonGeneric
        };

        private readonly Type _type = typeof(T);
        private readonly Type _listValueType;
        private readonly CreatorType _creatorType;
        private readonly ListAdapterFactoryType _listAdapterFactoryType;
        private readonly IDocumentFieldInfo _documentFieldInfo;

        private IListAdapterFactory? _listAdapterFactory;
        private Func<object>? _creator;

        public ListDocumentInfo(Type implementingType)
        {
            if (implementingType.IsGenericType)
            {
                _listValueType = implementingType.GetGenericArguments()[0];
                _documentFieldInfo = new DocumentFieldInfo(_listValueType);
                _listAdapterFactoryType = ListAdapterFactoryType.Generic;
                _creatorType = _type.IsInterface || _type.IsArray
                    ? CreatorType.SpecifiedTypeList : CreatorType.SpecifiedType;
            }
            else
            {
                _listValueType = typeof(object);
                _documentFieldInfo = new DocumentFieldInfo(_listValueType);
                _listAdapterFactoryType = ListAdapterFactoryType.NonGeneric;
                _creatorType = _type.IsInterface || _type.IsArray
                    ? CreatorType.ObjectList : CreatorType.SpecifiedType;
            }
        }

        public string GetMappingName(string name)
        {
            throw new NotSupportedException();
        }

        private object Create()
        {
            if (_creator == null)
            {
                _creator = _creatorType switch
                {
                    CreatorType.SpecifiedType => CreatorProvider.GetCreator<T>(),
                    CreatorType.SpecifiedTypeList => CreatorProvider.GetCreator(typeof(List<>).MakeGenericType(_listValueType)),
                    CreatorType.ObjectList => () => new List<object>(),
                    _ => throw new InvalidOperationException(),
                };
            }
            return _creator.Invoke();
        }

        private IListAdapter GetListAdapter(object target)
        {
            if (_listAdapterFactory == null)
            {
                _listAdapterFactory = _listAdapterFactoryType switch
                {
                    ListAdapterFactoryType.Generic => ObjectProvider.GetListAdapterFactory(_listValueType),
                    ListAdapterFactoryType.NonGeneric => ObjectProvider.GetListAdapterFactory(),
                    _ => throw new InvalidOperationException(),
                };
            }
            return _listAdapterFactory.Create(target);
        }
    }
}
