using System.Collections.Generic;
using System.Linq;
using System;
using System.Diagnostics;


public class CachingDic<K, V> : Dictionary<K, V>
{
    private class UnixTimeComparer : IComparer<long>
    {
        public int Compare(long x, long y)
        {
            return (x > y) ? 1 : -1;
        }

        public static IComparer<long> SortTimeAscending()
        { 
            return new UnixTimeComparer();
        }
    }

    private int capacity;
    private SortedList<long,K> accesTracker;
    private Dictionary<K, long> accesLib;

    public CachingDic(int capacity)
    {
        this.capacity = capacity;
        this.accesTracker = new SortedList<long,K>(UnixTimeComparer.SortTimeAscending());
        this.accesLib = new Dictionary<K, long>();
    }

    public void Cache(K key, V val)
    {
        if (Count > capacity)
        {
            KeyValuePair<long, K> removeEntry = accesTracker.First();
            Remove(removeEntry.Value);
            accesTracker.Remove(removeEntry.Key);
            accesLib.Remove(removeEntry.Value);
            UnityEngine.Debug.Log($"{removeEntry.Key}__f__{accesTracker.First().Key}__l__{accesTracker.Last().Key}");
            UnityEngine.Debug.Log($"");

        }
        Add(key, val);
        long now = DateTimeOffset.Now.ToUnixTimeSeconds();
        accesTracker.Add(now ,key);
        accesLib.Add(key, now);
    }

    public V Get(K key) {
        long now = DateTimeOffset.Now.ToUnixTimeSeconds();
        accesLib[key] = now;
        accesTracker.Add(now, key);
        return this[key];
    }

}