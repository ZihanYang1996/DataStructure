using System.Reflection.Metadata;

public class Entry<K, V>
{
    public int hash;
    public K key;
    public V value;

    public Entry(K key, V value)
    {
        this.key = key;
        this.value = value;
        this.hash = key.GetHashCode();
    }

    public bool Equals(Entry<K, V> other)
    {
        if (hash != other.hash)
        {
            return false;
        }
        return key.Equals(other.key);
    }

    public override string ToString()
    {
        return key + " => " + value;
    }
}

public class HashTableSeperateChaining<K, V>
{
    private static readonly int DEFAULT_CAPACITY = 4;
    private static readonly double DEFAULT_LOAD_FACTOR = 0.75;

    private double maxLoadFactor;
    private int capacity, threshold, size = 0;
    private LinkedList<Entry<K, V>>[] table;

    public HashTableSeperateChaining() : this(DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR)
    {

    }

    public HashTableSeperateChaining(int capacity) : this(DEFAULT_CAPACITY, DEFAULT_LOAD_FACTOR)
    {
    }

    public HashTableSeperateChaining(int capacity, double maxLoadFactor)
    {
        if (capacity < 0)
        {
            throw new System.ArgumentException("Illegal capacity");
        }
        if (maxLoadFactor <= 0 || double.IsNaN(maxLoadFactor) || double.IsInfinity(maxLoadFactor))
        {
            throw new System.ArgumentException("Illegal maxLoadFactor");
        }
        this.maxLoadFactor = maxLoadFactor;
        this.capacity = System.Math.Max(DEFAULT_CAPACITY, capacity);
        threshold = (int)(this.capacity * maxLoadFactor);
        table = new LinkedList<Entry<K, V>>[this.capacity];
    }

    public int Size()
    {
        return size;
    }

    public bool IsEmpty()
    {
        return size == 0;
    }
    // Converts a hash value to an index. Essentially, this strips the
    // negative sign and places the hash value in the domain [0, capacity)
    public int NormalizeIndex(int keyHash)
    {
        return (keyHash & 0x7FFFFFFF) % capacity;
    }

    public void Clear()
    {
        for (int i = 0; i < capacity; i++)
        {
            table[i] = null;
        }
        size = 0;
    }

    public bool ContainsKey(K key)
    {
        return HasKey(key);
    }

    public bool HasKey(K key)
    {
        int bucketIndex = NormalizeIndex(key.GetHashCode());
        return BucketSeekEntry(bucketIndex, key) != null;
    }

    public V Insert(K key, V value)
    {
        if (key == null)
        {
            throw new System.ArgumentException("Null key");
        }

        Entry<K, V> newEntry = new Entry<K, V>(key, value);
        int bucketIndex = NormalizeIndex(newEntry.hash);
        return BucketInsertEntry(bucketIndex, newEntry);
    }

    public V Get(K key)
    {
        if (key == null)
        {
            return default;
        }
        int bucketIndex = NormalizeIndex(key.GetHashCode());
        Entry<K, V> entry = BucketSeekEntry(bucketIndex, key);
        if (entry != null)
        {
            return entry.value;
        }
        return default;
    }

    public V Remove(K key)
    {
        if (key == null)
        {
            return default;
        }
        int bucketIndex = NormalizeIndex(key.GetHashCode());
        return BucketRemoveEntry(bucketIndex, key);
    }

    public List<K> Keys()
    {
        List<K> keys = new List<K>(size);
        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket)
                {
                    keys.Add(entry.key);
                }
            }
        }

        // for (int i = 0; i < capacity; i++)
        // {
        //     if (table[i] != null)
        //     {
        //         foreach (Entry<K, V> entry in table[i])
        //         {
        //             keys.Add(entry.key);
        //         }
        //     }
        // }
        return keys;
    }

    public List<V> Values()
    {
        List<V> values = new List<V>(size);
        foreach (var bucket in table)
        {
            if (bucket != null)
            {
                foreach (var entry in bucket)
                {
                    values.Add(entry.value);
                }
            }
        }
        return values;
    }

    private V BucketRemoveEntry(int bucketIndex, K key)
    {
        Entry<K, V> entry = BucketSeekEntry(bucketIndex, key);
        if (entry != null)
        {
            LinkedList<Entry<K, V>> links = table[bucketIndex];
            links.Remove(entry);
            --size;
            return entry.value;
        }
        return default;
    }

    private Entry<K, V> BucketSeekEntry(int bucketIndex, K key)
    {
        if (key == null) return null;
        LinkedList<Entry<K, V>> bucket = table[bucketIndex];
        if (bucket == null) return null;
        foreach (Entry<K, V> entry in bucket)
        {
            if (entry.key.Equals(key))
            {
                return entry;
            }
        }
        return null;
    }

    private V BucketInsertEntry(int bucketIndex, Entry<K, V> entry)
    {
        LinkedList<Entry<K, V>> bucket = table[bucketIndex];
        if (bucket == null)
        {
            table[bucketIndex] = bucket = new LinkedList<Entry<K, V>>();
        }
        Entry<K, V> existentEntry = BucketSeekEntry(bucketIndex, entry.key);
        if (existentEntry == null)
        {
            bucket.AddLast(entry);
            if (++size > threshold)
            {
                ResizeTable();
            }
            return default;
        }
        else
        {
            V oldVal = existentEntry.value;
            existentEntry.value = entry.value;
            return oldVal;
        }
    }

    private void ResizeTable()
    {
        capacity *= 2;
        threshold = (int)(capacity * maxLoadFactor);

        LinkedList<Entry<K, V>>[] newTable = new LinkedList<Entry<K, V>>[capacity];

        for (int i = 0; i < table.Length; i++)
        {
            if (table[i] != null)
            {
                foreach (Entry<K, V> entry in table[i])
                {
                    int bucketIndex = NormalizeIndex(entry.hash);
                    LinkedList<Entry<K, V>> bucket = newTable[bucketIndex];
                    if (bucket == null)
                    {
                        newTable[bucketIndex] = bucket = new LinkedList<Entry<K, V>>();
                    }
                    bucket.AddLast(entry);
                }
                table[i].Clear();
                table[i] = default;
            }
        }

        table = newTable;
    }
}

public class Program
{
    public static void Main()
    {
        HashTableSeperateChaining<string, int> hashTable = new HashTableSeperateChaining<string, int>();

        // Test Add method
        hashTable.Insert("One", 1);
        hashTable.Insert("Two", 2);
        hashTable.Insert("Three", 3);

        // Test ContainsKey method
        Console.WriteLine(hashTable.ContainsKey("One"));  // Should print True
        Console.WriteLine(hashTable.ContainsKey("Four")); // Should print False

        // Test Get method
        Console.WriteLine(hashTable.Get("One"));  // Should print 1
        Console.WriteLine(hashTable.Get("Two"));  // Should print 2

        // Test Remove method
        hashTable.Remove("One");
        Console.WriteLine(hashTable.ContainsKey("One"));  // Should print False

        // Test Keys method
        foreach (var key in hashTable.Keys())
        {
            Console.WriteLine(key);  // Should print "Two" and "Three"
        }

        // Test Clear method
        hashTable.Clear();
        Console.WriteLine(hashTable.ContainsKey("Two"));  // Should print False

    }
}