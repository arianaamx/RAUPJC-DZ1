using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prva_zadaća
{
    public interface IIntegerList
    {
            void Add(int item);
            ///Adds an item to the collection
            
            bool Remove(int item);
            /// Removes the first occurrence of an item from the collection .
            /// If the item was not found , method does nothing .
           
            bool RemoveAt(int index);
            /// Removes the item at the given index in the collection .

            
            int GetElement(int index);
            /// Returns the item at the given index in the collection .
            
            int IndexOf(int item);
            /// Returns the index of the item in the collection .
            /// If item is not found in the collection , method returns -1.

            int Count { get; }
            /// Readonly property . Gets the number of items contained in the collection.

            void Clear();
            ///Removes all items from the collection.
            
            bool Contains(int item);
            ///Determines wheter the collection contains a specific value.
            
            
    }

    public class IntegerList : IIntegerList
    {
        private int[] _internalStorage { get; set; }
        private static int _size { get; set; }
        private static int _lastElementIndex { get; set; }

        //KONSTRUKTORI! ---- Metode klase koje se izvršavaju kada se neki objekt stvori
        //              ---- Imaju isto ime kao i klasa i koriste se za inicijalizaciju podatkovnih članova novog objekta
        IntegerList()
        {
            _internalStorage = new int[4];
            _size = 4;
            _lastElementIndex = 0;
        }
        IntegerList(int initialSize)
        {
            _internalStorage = new int[initialSize];
            _size = initialSize;
            _lastElementIndex = 0;
        }

        public void Add(int item)
        {
            if (_lastElementIndex >= _size)
            {
                int[] pom = new int[2 * _size];
                int ind = 0;
                foreach (int i in _internalStorage)
                {
                    pom[ind] = i;
                    ind++;
                }
                _internalStorage = pom;
                _lastElementIndex = ind;
                _size = 2*_size;
            }
            _internalStorage[_lastElementIndex] = item;
            _lastElementIndex++;
        }

        public bool Remove(int item)
        {
            for (int ind = 0 ; ind <_size ; ind++) 
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
        }

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

        public int GetElement(int index)
        {
            for (int i = 0; i < _size; i++)
            {
                if (i+1 == index)
                    return _internalStorage[i];
            }
            return 0;
        }

        public int IndexOf(int item)
        {
            for (int i=0; i < _size; i++)
            {
                if (_internalStorage[i] == item)
                    return (i + 1);
            }
            return -1;
        }

        public int Count {
            get { return _lastElementIndex; } }

        public void Clear()
        {
            for (int i = 0; i<_size; i++)
            {
                _internalStorage[i] = 0;
            }
        }

        public bool Contains (int item)
        {
            for (int i = 0; i < _size; i++)
            {
                if (_internalStorage[i] == item)
                    return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            IntegerList pom = new IntegerList();
            pom.Add(1);
            pom.Add(2);
            pom.Add(3);
            pom.Add(4);
            pom.Add(5);
            foreach (int i in pom._internalStorage)
            {
                Console.WriteLine(i);
            } Console.WriteLine(" ");

            pom.RemoveAt(0);
            foreach (int i in pom._internalStorage)
            {
                Console.WriteLine(i);
            } Console.WriteLine(" ");

            pom.Remove(5);
            foreach (int i in pom._internalStorage)
            {
                Console.WriteLine(i);
            } Console.WriteLine(" ");

            Console.WriteLine(pom.GetElement(2));
            Console.WriteLine(" ");

            Console.WriteLine(pom.IndexOf(3));
            Console.WriteLine(" ");

            Console.WriteLine(pom.Count);
            Console.WriteLine(" ");

            pom.Clear();
            foreach (int i in pom._internalStorage)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine(" ");

            Console.WriteLine(pom.Contains(3));

            Console.ReadLine();

        }
    }
}
