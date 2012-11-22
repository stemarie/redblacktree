using System.IO;
using System.Xml.Serialization;

namespace System.Collections.Generic.RedBlack.Beta
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class RedBlackTreePersistent<TKey, TValue>
        : RedBlackTree<TKey, TValue>
        where TKey : IComparable
        where TValue : class
    {
        private readonly string _path;
        private readonly XmlSerializer _serializer;

        public RedBlackTreePersistent(string path)
        {
            _path = path;
            _serializer = new XmlSerializer(typeof(SerializedItem<TKey, TValue>));
            foreach (string file in Directory.EnumerateFiles(_path, "*" + typeof(TValue)))
            {
                using (StreamReader fileStream = File.OpenText(file))
                {
                    SerializedItem<TKey, TValue> item = (SerializedItem<TKey, TValue>)_serializer.Deserialize(fileStream);
                    base.Add(item.Key, item.Value);
                }
            }
        }

        public override void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            SaveToFile(key, value);
        }

        public override void Add(KeyValuePair<TKey, TValue> item)
        {
            base.Add(item);
            SaveToFile(item.Key, item.Value);
        }

        public override bool Remove(TKey key)
        {
            DeleteFromFile(key);
            return base.Remove(key);
        }

        public override bool Remove(KeyValuePair<TKey, TValue> item)
        {
            DeleteFromFile(item.Key);
            return base.Remove(item);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private void SaveToFile(TKey key, TValue value)
        {
            using (StreamWriter file = File.CreateText(GetFullFileName(key)))
            {
                _serializer.Serialize(file, new SerializedItem<TKey, TValue>(key, value));
                file.Close();
            }
        }

        private string GetFullFileName(TKey key)
        {
            string fullPath = Path.Combine(_path, GetFileName(key));
            return fullPath;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1305:SpecifyIFormatProvider", MessageId = "System.String.Format(System.String,System.Object,System.Object,System.Object)")]
        private static string GetFileName(TKey key)
        {
            return string.Format("{0}-{1}.{2}", key, typeof(TValue), "xml");
        }

        private void DeleteFromFile(TKey key)
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
