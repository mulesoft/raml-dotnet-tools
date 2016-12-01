using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Raml.Parser.Expressions
{
    public class RamlTypesOrderedDictionary
    {
        private readonly OrderedDictionary dic = new OrderedDictionary();
        public int Count { get { return dic.Count; } }

        public void Clear()
        {
            dic.Clear();
        }

        public List<string> Keys { get { return dic.Keys.Cast<string>().ToList(); } }

        public IDictionaryEnumerator GetEnumerator()
        {
            return dic.GetEnumerator();
        }

        public void Add(string key, RamlType value)
        {
            dic.Add(key, value);
        }

        public RamlType GetByKey(string key)
        {
            if (!ContainsKey(key))
                return null;

            var type = dic[key] as RamlType;
            return type;
        }

        public bool ContainsKey(string key)
        {
            return dic.Contains(key);
        }

        public RamlType this[string key]
        {
            get { return (RamlType)dic[key]; }
        }
    }
}