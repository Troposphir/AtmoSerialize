using System.Collections;
using System.Collections.Generic;

namespace AtmoSerialize {
    public class Rulebook : IDictionary<string, IAtmoProperty> {
        private readonly IDictionary<string, IAtmoProperty> _rules;

        public Rulebook() : this(new Dictionary<string, IAtmoProperty>()) {}
        public Rulebook(IDictionary<string, IAtmoProperty> rules) {
            _rules = rules;
        }

        public T Get<T>(string key) where T : IAtmoProperty {
            return (T) this[key];
        }

        public IEnumerator<KeyValuePair<string, IAtmoProperty>> GetEnumerator() => _rules.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_rules).GetEnumerator();
        public void Add(KeyValuePair<string, IAtmoProperty> item) => _rules.Add(item);
        public void Clear() => _rules.Clear();
        public bool Contains(KeyValuePair<string, IAtmoProperty> item) => _rules.Contains(item);
        public void CopyTo(KeyValuePair<string, IAtmoProperty>[] array, int arrayIndex) => _rules.CopyTo(array, arrayIndex);
        public bool Remove(KeyValuePair<string, IAtmoProperty> item) => _rules.Remove(item);
        public int Count => _rules.Count;
        public bool IsReadOnly => false;
        public bool ContainsKey(string key) => _rules.ContainsKey(key);
        public void Add(string key, IAtmoProperty value) => _rules.Add(key, value);
        public bool Remove(string key) => _rules.Remove(key);
        public bool TryGetValue(string key, out IAtmoProperty value) => _rules.TryGetValue(key, out value);
        public ICollection<string> Keys => _rules.Keys;
        public ICollection<IAtmoProperty> Values => _rules.Values;
        public IAtmoProperty this[string key] {
            get { return _rules[key]; }
            set { _rules[key] = value; }
        }
    }
}