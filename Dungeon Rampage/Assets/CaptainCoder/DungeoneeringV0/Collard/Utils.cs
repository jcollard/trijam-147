namespace Collard
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    
    public class Utils
    {

        public static IEnumerable<String> GetStringIterable(string text)
        {
            string line;
            using StringReader reader = new StringReader(text);
            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }

            reader.Close();
        }

        public static IEnumerable<(K, V)> GetKeyValueEnumerable<K, V>(Dictionary<K, V> dict)
        {
            foreach (K key in dict.Keys)
            {
                yield return (key, dict[key]);
            }
        }

        public static Dictionary<K, V> ToDict<K, V>(IEnumerable<(K, V)> data)
        {
            Dictionary<K, V> d = new Dictionary<K, V>();
            if (data == null)
            {
                return d;
            }
            foreach ((K key, V value) in data)
            {
                d[key] = value;
            }
            return d;
        }

        public static IEnumerable<(int, T)> GetIndexEnumerable<T>(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                yield return (i, arr[i]);
            }
        }

        public static IEnumerable<(int, int, T)> Get2DEnumerable<T>(T[,] arr)
        {
            for (int r = 0; r < arr.GetLength(0); r++)
            {
                for (int c = 0; c < arr.GetLength(1); c++)
                {
                    yield return (r, c, arr[r, c]);
                }
            }
        }
    }
}