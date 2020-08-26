using System;
using System.Collections;
using System.Collections.Generic;

namespace Plugin.CloudFirestore
{
    internal interface IDictionaryAdapter : IEnumerable
    {
        object? this[object key] { get; set; }
        new IDictionaryEnumerator GetEnumerator();
    }

    internal class DictionaryAdapter<TKey, TValue> : IDictionaryAdapter
    {
        private readonly IDictionary<TKey, TValue>? _dictionary;
        private readonly IReadOnlyDictionary<TKey, TValue>? _readonlyDictionary;

        public DictionaryAdapter(IDictionary<TKey, TValue> dictionary)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        public DictionaryAdapter(IReadOnlyDictionary<TKey, TValue> readonlyDictionary)
        {
            _readonlyDictionary = readonlyDictionary ?? throw new ArgumentNullException(nameof(readonlyDictionary));
        }

        public object? this[object key]
        {
            get
            {
                if (_readonlyDictionary != null)
                {
                    return _readonlyDictionary[(TKey)key];
                }
                return _dictionary![(TKey)key];
            }
            set
            {
                if (_readonlyDictionary != null)
                {
                    throw new NotSupportedException();
                }
                _dictionary![(TKey)key] = (TValue)value!;
            }
        }

        IDictionaryEnumerator IDictionaryAdapter.GetEnumerator()
        {
            if (_readonlyDictionary != null)
            {
                return new DictionaryEnumerator(_readonlyDictionary.GetEnumerator());
            }
            return new DictionaryEnumerator(_dictionary!.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (_readonlyDictionary != null)
            {
                return ((IEnumerable)_readonlyDictionary).GetEnumerator();
            }
            return ((IEnumerable)_dictionary!).GetEnumerator();
        }

        private class DictionaryEnumerator : IDictionaryEnumerator
        {
            private readonly IEnumerator<KeyValuePair<TKey, TValue>> _enumerator;

            public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
            {
                _enumerator = enumerator;
            }

            public DictionaryEntry Entry => (DictionaryEntry)Current;

            public object Key => Entry.Key;

            public object Value => Entry.Value;

            public object Current => new DictionaryEntry(_enumerator.Current.Key, _enumerator.Current.Value);

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }
    }

    internal class DictionaryAdapter : IDictionaryAdapter
    {
        private readonly IDictionary _dictionary;

        public object? this[object key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }

        public DictionaryAdapter(IDictionary dictionary)
        {
            _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dictionary).GetEnumerator();
        }
    }
}
