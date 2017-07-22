using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace _2DRabbitPlatform.Engine
{
    public class DoubleArray<TKey, TVal>
    {
        List<ValKey> keyvalues;
        KeyCollection keys;
        ValueCollection values;

        public DoubleArray()
        {
            keyvalues = new List<ValKey>();
            keys = new KeyCollection(keyvalues);
            values = new ValueCollection(keyvalues);
        }

        public void Put(TKey key, TVal value)
        {
            if (!ContainsKey(key))
                keyvalues.Add(new ValKey() { key = key, val = value });
            else
            {
                keyvalues[findIndex(key)].val = value;
            }
        }

        public bool ContainsKey(TKey key)
        {
            foreach (TKey curkey in keys)
            {
                if (curkey.Equals(key))
                    return true;
            }

            return false;
        }

        public bool ContainsValue(TVal value)
        {
            foreach (TVal curval in values)
            {
                if (curval.Equals(value))
                    return true;
            }

            return false;
        }

        private int findIndex(TKey key)
        {
            for(int i = 0; i < keyvalues.Count; i++)
                if(keyvalues[i].key.Equals(key))
                    return i;

            return -1;
        }

        public TVal this[TKey key]
        {
            get
            {
                return keyvalues[findIndex(key)].val;
            }
        }

        public KeyCollection Keys { get { return keys; } }
        public ValueCollection Values { get { return values; } }

        public class ValKey
        {
            public TKey key;
            public TVal val;
        }

        public class KeyCollection : IEnumerator<TKey>, IEnumerable
        {
            List<ValKey> keys;
            int position;

            public KeyCollection(List<ValKey> keys)
            {
                this.keys = keys;
                this.position = -1;
            }

            public bool MoveNext()
            {
                position++;
                return position < keys.Count;
            }

            public void Reset()
            {
                position = -1;
            }

            public TKey Current
            {
                get { return keys[position].key; }
            }

            public void Dispose()
            {
                
            }

            public IEnumerator GetEnumerator()
            {
                return new KeyCollection(keys);
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }

        public class ValueCollection : IEnumerator<TVal>, IEnumerable
        {
            List<ValKey> keys;
            int position;

            public ValueCollection(List<ValKey> keys)
            {
                this.keys = keys;
                this.position = -1;
            }

            public bool MoveNext()
            {
                position++;
                return position < keys.Count;
            }

            public void Reset()
            {
                position = -1;
            }

            public TVal Current
            {
                get { return keys[position].val; }
            }

            public void Dispose()
            {
                
            }

            public IEnumerator GetEnumerator()
            {
                return new ValueCollection(keys);
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }
        }
    }
}
