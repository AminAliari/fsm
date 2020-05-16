using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS_FinalProject {

    public class HashElement<K, V> {

        public K key;
        public V value;

        public HashElement(K key, V value) {
            this.key = key;
            this.value = value;
        }
    }

    public class Hash<K, V> {

        public int length;
        const int TABLE_SIZE = 512;

        List<List<HashElement<K, V>>> table;

        public Hash() {
            length = 0;
            table = new List<List<HashElement<K, V>>>(TABLE_SIZE);
            for (int i = 0; i < TABLE_SIZE; i++) {
                table.Add(new List<HashElement<K, V>>());
            }
        }

        int getHash(K key) {
            return Math.Abs(key.GetHashCode() % TABLE_SIZE);
        }

        public HashElement<K, V> get(K key) {
            int hash = getHash(key);
            foreach (HashElement<K, V> t in table[hash]) {
                if (t.key.Equals(key)) {
                    return t;
                }
            }
            return null;
        }

        public void put(K key, V value) {
            int hash = getHash(key);
            switch (contains(key, value, hash)) {
                case 0:
                    table[hash].Add(new HashElement<K, V>(key, value));
                    length++;
                    break;

                case 1:
                    get(key).value = value;
                    break;
            }

        }

        public void remove(K key) {
            int hash = getHash(key);
            foreach (HashElement<K, V> t in table[hash]) {
                if (t.key.Equals(key)) {
                    table[hash].Remove(t);
                    break;
                }
            }
        }

        int contains(K key, V value, int hash) {
            foreach (HashElement<K, V> t in table[hash]) {
                if (t.key.Equals(key) && t.value.Equals(value)) {
                    return 2;
                } else if (t.key.Equals(key)) {
                    return 1;
                }
            }
            return 0;
        }
    }
}
