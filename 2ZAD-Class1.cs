using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary2
{
    public interface IGenericList <X> : IEnumerable<X>
    {
        void Add(X item);
        bool Remove(X index);
        bool RemoveAt(int index);
        X GetElement(int index);
        int IndexOf(X item);
        int Count { get; }
        void Clear();
        bool Contains(X item);
     }

    public class GenericList<X> : IGenericList <X> where X : IComparable<X>
    {
        private X[] _internalStorage;
        private int _size;
        private int _lastElementIndex;

        public GenericList()
        {
            _internalStorage = new X[4];
            _size = 4;
            _lastElementIndex = 0;
        }
        public GenericList(int initialSize)
        {
            _internalStorage = new X[initialSize];
            _size = initialSize;
            _lastElementIndex = 0;
        }

        public void Add(X item)
        {
            if (_lastElementIndex >= _size)
            {
                X[] pom = new X[2 * _size];
                int ind = 0;
                foreach (X i in _internalStorage)
                {
                    pom[ind] = i;
                    ind++;
                }
                _internalStorage = pom;
                _lastElementIndex = ind;
                _size = 2 * _size;
            }
            _internalStorage[_lastElementIndex] = item;
            _lastElementIndex++;
        }

        /*public bool Remove(X item)
        {
            for (int ind = 0; ind < _size; ind++)
            {
                if (_internalStorage[ind] == item)
                {
                    for (int ind2 = ind + 1; ind2 < _size; ind2++)
                    {
                        _internalStorage[ind] = _internalStorage[ind2];
                        ind++;
                    }
                    _lastElementIndex--;
                    return true;
                }
            }
            return false;
        }*/

        public bool RemoveAt(int index)
        {
            if (index < _size)
            {
                for (int i = index + 1; i < _size; i++)
                {
                    _internalStorage[index] = _internalStorage[i];
                    index++;
                }
                _lastElementIndex--;
                return true;
            }
            return false;
        }

        public X GetElement(int index)
        {
            if (index >= 0 && index <= _size)
            {
                return _internalStorage[index];
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public int IndexOf(X item)
        {
            int index = 0;
            foreach (X value in _internalStorage)
            {
                if (item.CompareTo(value) == 0)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public void Clear()
        {
            _internalStorage = null;
            _lastElementIndex = -1;
        }

        public bool Contains(X item)
        {
            if (IndexOf(item) > -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(X item)
        {
            return RemoveAt(IndexOf(item));
        }

        public IEnumerator<X> GetEnumerator()
        {
            return new GenericListEnumerator<X>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /*public X GetElement(int index)
        {
            for (int i = 0; i < _size; i++)
            {
                if (i + 1 == index)
                    return _internalStorage[i];
            }
            return 0;
        }*/

        /*public int IndexOf(X item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_internalStorage[i] == item)
                    return (i + 1);
            }
            return -1;
        }
        */
        public int Count
        {
            get { return _lastElementIndex; }
        }

        /*public void Clear()
        {
            for (int i = 0; i < _size; i++)
            {
                _internalStorage[i] = 0;
            }
        }

        public bool Contains(X item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_internalStorage[i] == item)
                    return true;
            }
            return false;
        }*/
    }

    public class GenericListEnumerator<X> : IEnumerator<X> where X : IComparable<X>
    {
        // After hours of searching 
        // http://www.codeproject.com/Articles/21241/Implementing-C-Generic-Collections-using-ICollecti
        private GenericList<X> _collection = new GenericList<X>();     // enumered collection
        private int index; // curent index
        private X _current;

        // Constructor
        public GenericListEnumerator()
        {

        }

        public GenericListEnumerator(GenericList<X> collection)
        {
            _collection = collection;
            index = -1;
            _current = default(X);
        }



        public X Current
        {
            get
            {
                return _current;
            }
        }


        object IEnumerator.Current
        {
            get
            {
                return _current;
            }
        }
        public void Dispose()
        {
            _collection = null;
            _current = default(X);
            index = -1;
        }

        public bool MoveNext()
        {
            if (++index >= _collection.Count)
            {
                return false;
            }
            else
            {
                _current = _collection.GetElement(index);
            }
            return true;
        }

        public void Reset()
        {
            _current = default(X);
            index = -1;
        }
    }
}





