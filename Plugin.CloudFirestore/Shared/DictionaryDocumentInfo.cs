using System;
using System.Collections;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal partial class DictionaryDocumentInfo<T> : IDocumentInfo
    {
        private enum CreatorType
        {
            SpecifiedType,
            SpecifiedTypeDictionary,
            ObjectDictionary
        };

        private enum DictionaryAdapterFactoryType
        {
            Generic,
            NonGeneric
        };

        private readonly Type _type = typeof(T);
        private readonly Type _dictionaryKeyType;
        private readonly Type _dictionaryValueType;
        private readonly CreatorType _creatorType;
        private readonly DictionaryAdapterFactoryType _dictionaryAdapterFactoryType;
        private readonly IDocumentFieldInfo _documentFieldInfo;

        private Func<object>? _creator;
        private IDictionaryAdapterFactory? _dictionaryAdapterFactory;

        public DictionaryDocumentInfo(Type implementingType)
        {
            if (implementingType.IsGenericType)
            {
                var arguments = implementingType.GetGenericArguments();
                _dictionaryKeyType = arguments[0];
                _dictionaryValueType = arguments[1];
                _documentFieldInfo = new DocumentFieldInfo(_dictionaryValueType);
                _dictionaryAdapterFactoryType = DictionaryAdapterFactoryType.Generic;
                _creatorType = _type.IsInterface
                    ? CreatorType.SpecifiedTypeDictionary : CreatorType.SpecifiedType;
            }
            else
            {
                _dictionaryKeyType = typeof(string);
                _dictionaryValueType = typeof(object);
                _documentFieldInfo = new DocumentFieldInfo(_dictionaryValueType);
                _dictionaryAdapterFactoryType = DictionaryAdapterFactoryType.NonGeneric;
                _creatorType = _type.IsInterface
                    ? CreatorType.ObjectDictionary : CreatorType.SpecifiedType;
            }
        }

        public string GetMappingName(string name)
        {
            return name;
        }

        private object Create()
        {
            if (_creator == null)
            {
                _creator = _creatorType switch
                {
                    CreatorType.SpecifiedType => CreatorProvider.GetCreator<T>(),
                    CreatorType.SpecifiedTypeDictionary => CreatorProvider.GetCreator(typeof(Dictionary<,>).MakeGenericType(_dictionaryKeyType, _dictionaryValueType)),
                    CreatorType.ObjectDictionary => () => new Dictionary<string, object>(),
                    _ => throw new InvalidOperationException(),
                };
            }
            return _creator.Invoke();
        }

        private IDictionaryAdapter GetDictionaryAdapter(object target)
        {
            if (_dictionaryAdapterFactory == null)
            {
                _dictionaryAdapterFactory = _dictionaryAdapterFactoryType switch
                {
                    DictionaryAdapterFactoryType.Generic => ObjectProvider.GetDictionaryAdapterFactory(_dictionaryKeyType, _dictionaryValueType),
                    DictionaryAdapterFactoryType.NonGeneric => ObjectProvider.GetDictionaryAdapterFactory(),
                    _ => throw new InvalidOperationException(),
                };
            }
            return _dictionaryAdapterFactory.Create(target);
        }
    }
}
