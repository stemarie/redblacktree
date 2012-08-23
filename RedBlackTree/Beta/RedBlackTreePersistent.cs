using System.IO;
using System.Xml.Serialization;

namespace System.Collections.Generic.RedBlack.Beta
{
    public class RedBlackTreePersistent<K, T>
        : RedBlackTree<K, T>
        where K : IComparable
        where T : class
    {
        private readonly string _path;
        private readonly XmlSerializer _serializer;

        public RedBlackTreePersistent(string path)
        {
            _path = path;
            _serializer = new XmlSerializer(typeof(SerializedItem<K, T>));
            foreach (string file in Directory.EnumerateFiles(_path, "*" + typeof(T)))
            {
                using (StreamReader fileStream = File.OpenText(file))
                {
                    SerializedItem<K, T> item = (SerializedItem<K, T>)_serializer.Deserialize(fileStream);
                    base.Add(item.Key, item.Value);
                }
            }
        }

        public override void Add(K key, T value)
        {
            base.Add(key, value);
            SaveToFile(key, value);
        }

        public override void Add(KeyValuePair<K, T> item)
        {
            base.Add(item);
            SaveToFile(item.Key, item.Value);
        }

        public override bool Remove(K key)
        {
            DeleteFromFile(key);
            return base.Remove(key);
        }

        public override bool Remove(KeyValuePair<K, T> item)
        {
            DeleteFromFile(item.Key);
            return base.Remove(item);
        }

        private void SaveToFile(K key, T value)
        {
            using (StreamWriter file = File.CreateText(GetFullFileName(key)))
            {
                _serializer.Serialize(file, new SerializedItem<K, T>(key, value));
                file.Close();
            }
        }

        private string GetFullFileName(K key)
        {
            string fullPath = Path.Combine(_path, GetFileName(key));
            return fullPath;
        }

        private static string GetFileName(K key)
        {
            return string.Format("{0}-{1}.{2}", key, typeof(T), "xml");
        }

        private void DeleteFromFile(K key)
        {
            File.Delete(GetFullFileName(key));
        }

        internal class SerializedItem<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }

            public SerializedItem()
            { }

            public SerializedItem(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }
    }
}
