using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Plugin.CloudFirestore
{
    internal interface IListAdapter : IEnumerable
    {
        void Add(object item);
        Array ToArray();
    }

    internal class ListAdapter<T> : IListAdapter
    {
        private readonly IList<T> _list;
        private readonly IReadOnlyList<T> _readonlyList;

        public ListAdapter(IList<T> list)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public ListAdapter(IReadOnlyList<T> readonlyList)
        {
            _readonlyList = readonlyList ?? throw new ArgumentNullException(nameof(readonlyList));
        }

        public void Add(object item)
        {
            if (_readonlyList != null)
            {
                throw new NotSupportedException();
            }
            _list.Add((T)item);
        }

        public IEnumerator GetEnumerator()
        {
            if (_readonlyList != null)
            {
                return _readonlyList.GetEnumerator();
            }
            return _list.GetEnumerator();
        }

        public Array ToArray()
        {
            return _list.ToArray();
        }
    }

    internal class ListAdapter : IListAdapter
    {
        private readonly IList _list;

        public ListAdapter(IList list)
        {
            _list = list ?? throw new ArgumentNullException(nameof(list));
        }

        public void Add(object item)
        {
            _list.Add(item);
        }

        public IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public Array ToArray()
        {
            var array = new object[_list.Count];
            _list.CopyTo(array, 0);
            return array;
        }
    }
}
