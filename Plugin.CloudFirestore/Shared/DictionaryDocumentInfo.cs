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

        private Func<object> _creator;
        private IDictionaryAdapterFactory _dictionaryAdapterFactory;

        public DictionaryDocumentInfo()
        {
            if (_type.TryGetImplementingGenericType(out var dictionaryType, typeof(IDictionary<,>)) ||
                _type.TryGetImplementingGenericType(out dictionaryType, typeof(IReadOnlyDictionary<,>)))
            {
                var arguments = dictionaryType.GetGenericArguments();
                _dictionaryKeyType = arguments[0];
                _dictionaryValueType = arguments[1];
                _documentFieldInfo = new DocumentFieldInfo(_dictionaryValueType);

                if (_type == dictionaryType)
                {
                    _creatorType = CreatorType.SpecifiedTypeDictionary;
                    _dictionaryAdapterFactoryType = DictionaryAdapterFactoryType.NonGeneric;
                }
                else
                {
                    _creatorType = CreatorType.SpecifiedType;
                    _dictionaryAdapterFactoryType = DictionaryAdapterFactoryType.Generic;
                }
            }
            else if (typeof(IDictionary).IsAssignableFrom(_type))
            {
                _dictionaryKeyType = typeof(string);
                _dictionaryValueType = typeof(object);
                _documentFieldInfo = new DocumentFieldInfo(_dictionaryValueType);

                if (_type == typeof(IDictionary))
                {
                    _creatorType = CreatorType.ObjectDictionary;
                }
                else
                {
                    _creatorType = CreatorType.SpecifiedType;
                }
                _dictionaryAdapterFactoryType = DictionaryAdapterFactoryType.NonGeneric;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public object ConvertToFieldObject(object target)
        {
#if NETSTANDARD
            throw new NotImplementedException();
#else
            return PlatformConvertToFieldObject(target);
#endif
        }

        public object ConvertToFieldValue(object target)
        {
#if NETSTANDARD
            throw new NotImplementedException();
#else
            return PlatformConvertToFieldValue(target);
#endif
        }

        public object Create(object value, ServerTimestampBehavior? serverTimestampBehavior = null)
        {
#if NETSTANDARD
            throw new NotImplementedException();
#else
            return PlatformCreate(value, serverTimestampBehavior);
#endif
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
