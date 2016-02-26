using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
    public class RamlTypesOrderedDictionary
    {
        private readonly OrderedDictionary dic = new OrderedDictionary();
        public int Count { get { return dic.Count; } }

        public void Clear()
        {
            dic.Clear();
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

        // TODO: change !!!!
        [Obsolete("Change parser for ordered dictionary !!")]
        public IDictionary<string, RamlType> ToDictionary()
        {
            var dictionary = new Dictionary<string, RamlType>();
            foreach (var key in dic.Keys)
            {
                dictionary.Add(key.ToString(), (RamlType)dic[key]);
            }
            return dictionary;
        }
    }
}