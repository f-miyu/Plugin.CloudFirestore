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

        public ListDocumentInfo()
        {
            if (_type.TryGetImplementingGenericType(out var listType, typeof(IList<>)) ||
                _type.TryGetImplementingGenericType(out listType, typeof(IReadOnlyList<>)))
            {
                _listValueType = listType.GetGenericArguments()[0];
                _documentFieldInfo = new DocumentFieldInfo(_listValueType);

                if (_type == listType)
                {
                    _creatorType = CreatorType.SpecifiedTypeList;
                    _listAdapterFactoryType = ListAdapterFactoryType.NonGeneric;
                }
                else if (_type.IsArray)
                {
                    _creatorType = CreatorType.SpecifiedTypeList;
                    _listAdapterFactoryType = ListAdapterFactoryType.Generic;
                }
                else
                {
                    _creatorType = CreatorType.SpecifiedType;
                    _listAdapterFactoryType = ListAdapterFactoryType.Generic;
                }
            }
            else if (typeof(IList).IsAssignableFrom(_type))
            {
                _listValueType = typeof(object);
                _documentFieldInfo = new DocumentFieldInfo(_listValueType);

                if (_type == typeof(IList) || _type.IsArray)
                {
                    _creatorType = CreatorType.ObjectList;
                }
                else
                {
                    _creatorType = CreatorType.SpecifiedType;
                }
                _listAdapterFactoryType = ListAdapterFactoryType.NonGeneric;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public object ConvertToFieldObject(object target)
        {
            throw new NotSupportedException();
        }

        public object ConvertToFieldValue(object target)
        {
#if NETSTANDARD
            throw new NotImplementedException();
#else
            return PlatformConvertToFieldValue(target);
#endif
        }

        public object? Create(object? value, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
#if NETSTANDARD
            throw new NotImplementedException();
#else
            return PlatformCreate(value, serverTimestampBehavior);
#endif
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
