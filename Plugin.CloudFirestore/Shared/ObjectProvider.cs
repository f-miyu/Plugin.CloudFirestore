using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Plugin.CloudFirestore
{
    internal static class ObjectProvider
    {
        private static class DocumentInfoCache<T>
        {
            public static readonly IDocumentInfo Instance = new DocumentInfo<T>();
        }

        private static class ListAdapterFactoryCache<T>
        {
            public static readonly IListAdapterFactory Instance = new ListAdapterFactory<T>();
        }

        private static class DictionaryAdapterFactoryCache<TKey, TValue>
        {
            public static readonly IDictionaryAdapterFactory Instance = new DictionaryAdapterFactory<TKey, TValue>();
        }

        private static readonly ConcurrentDictionary<Type, IDocumentInfo> _documentInfos = new ConcurrentDictionary<Type, IDocumentInfo>();
        private static readonly ConcurrentDictionary<Type, IListAdapterFactory> _listAdapterFactories = new ConcurrentDictionary<Type, IListAdapterFactory>();
        private static readonly ConcurrentDictionary<(Type, Type), IDictionaryAdapterFactory> _dictionaryAdapterFactories = new ConcurrentDictionary<(Type, Type), IDictionaryAdapterFactory>();

        private static readonly IListAdapterFactory _listAdapterFactory = new ListAdapterFactory();
        private static readonly IDictionaryAdapterFactory _dictionaryAdapterFactory = new DictionaryAdapterFactory();

        public static IDocumentInfo GetDocumentInfo(Type documentType)
        {
            return _documentInfos.GetOrAdd(documentType, GetDocumentInfoCache);
        }

        public static IDocumentInfo GetDocumentInfo<T>()
        {
            return DocumentInfoCache<T>.Instance;
        }

        public static IListAdapterFactory GetListAdapterFactory(Type type)
        {
            return _listAdapterFactories.GetOrAdd(type, GetListAdapterFactoryCache);
        }

        public static IListAdapterFactory GetListAdapterFactory<T>()
        {
            return ListAdapterFactoryCache<T>.Instance;
        }

        public static IListAdapterFactory GetListAdapterFactory()
        {
            return _listAdapterFactory;
        }

        public static IDictionaryAdapterFactory GetDictionaryAdapterFactory(Type keyType, Type valueType)
        {
            return _dictionaryAdapterFactories.GetOrAdd((keyType, valueType), GetDictionaryAdapterFactoryCache);
        }

        public static IDictionaryAdapterFactory GetDictionaryAdapterFactory<TKey, TValue>()
        {
            return DictionaryAdapterFactoryCache<TKey, TValue>.Instance;
        }

        public static IDictionaryAdapterFactory GetDictionaryAdapterFactory()
        {
            return _dictionaryAdapterFactory;
        }

        private static IDocumentInfo GetDocumentInfoCache(Type type)
        {
            return (IDocumentInfo)typeof(DocumentInfoCache<>).MakeGenericType(type)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)
                .GetValue(null);
        }

        private static IListAdapterFactory GetListAdapterFactoryCache(Type type)
        {
            return (IListAdapterFactory)typeof(ListAdapterFactoryCache<>).MakeGenericType(type)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)
                .GetValue(null);
        }

        private static IDictionaryAdapterFactory GetDictionaryAdapterFactoryCache((Type key, Type value) type)
        {
            return (IDictionaryAdapterFactory)typeof(DictionaryAdapterFactoryCache<,>).MakeGenericType(type.key, type.value)
                .GetField("Instance", BindingFlags.Public | BindingFlags.Static)
                .GetValue(null);
        }
    }
}
