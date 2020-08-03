using System;
using System.Collections;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    interface IDictionaryAdapterFactory
    {
        IDictionaryAdapter Create(object target);
    }

    internal class DictionaryAdapterFactory<TKey, TValue> : IDictionaryAdapterFactory
    {
        public IDictionaryAdapter Create(object target)
        {
            return target switch
            {
                IDictionary<TKey, TValue> dictionary => new DictionaryAdapter<TKey, TValue>(dictionary),
                IReadOnlyDictionary<TKey, TValue> dictionary => new DictionaryAdapter<TKey, TValue>(dictionary),
                _ => throw new ArgumentOutOfRangeException(nameof(target))
            };
        }
    }

    internal class DictionaryAdapterFactory : IDictionaryAdapterFactory
    {
        public IDictionaryAdapter Create(object target)
        {
            return target switch
            {
                IDictionary dictionary => new DictionaryAdapter(dictionary),
                _ => throw new ArgumentOutOfRangeException(nameof(target))
            };
        }
    }
}
