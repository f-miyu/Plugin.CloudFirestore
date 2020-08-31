using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    internal interface IListAdapter : IEnumerable
    {
        void Add(object? item);
        Array ToArray();
    }

    internal class ListAdapter<T> : IListAdapter
    {
        private readonly ICollection<T>? _collection;
        private readonly IEnumerable<T>? _enumerable;

        public ListAdapter(ICollection<T> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        public ListAdapter(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
        }

        public void Add(object? item)
        {
            if (_enumerable != null)
            {
                throw new NotSupportedException();
            }
            _collection!.Add((T)item!);
        }

        public IEnumerator GetEnumerator()
        {
            if (_enumerable != null)
            {
                return _enumerable.GetEnumerator();
            }
            return _collection!.GetEnumerator();
        }

        public Array ToArray()
        {
            if (_enumerable != null)
            {
                return _enumerable.ToArray();
            }
            return _collection.ToArray();
        }
    }

    internal class ListAdapter : IListAdapter
    {
        private readonly IList? _list;
        private readonly IEnumerable? _enumerable;

        public ListAdapter(IList list)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public ListAdapter(IEnumerable enumerable)
        {
            _enumerable = enumerable ?? throw new ArgumentNullException(nameof(enumerable));
        }

        public void Add(object? item)
        {
            if (_enumerable != null)
            {
                throw new NotSupportedException();
            }
            _list!.Add(item);
        }

        public IEnumerator GetEnumerator()
        {
            if (_enumerable != null)
            {
                return _enumerable.GetEnumerator();
            }
            return _list!.GetEnumerator();
        }

        public Array ToArray()
        {
            if (_enumerable != null)
            {
                var list = new List<object>();
                foreach (var value in _enumerable)
                {
                    list.Add(value);
                }
                return list.ToArray();
            }
            var array = new object[_list!.Count];
            _list.CopyTo(array, 0);
            return array;
        }
    }
}
