using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace James.Testing.Rest
{
    internal class DynamicDictionary : DynamicObject
    {
        private readonly Dictionary<string, object> _dictionary;

        public DynamicDictionary(object value)
            : this(new Dictionary<string, object>())
        {
            foreach (var property in value.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                _dictionary[property.Name] = property.GetValue(value, null);
            }
        }

        public DynamicDictionary()
            : this(new Dictionary<string, object>())
        {}

        public DynamicDictionary(Dictionary<string, object> dictionary)
        {
            this._dictionary = dictionary;
        }

        public static DynamicDictionary FromObject(object value)
        {
            return new DynamicDictionary(value);
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;

            return true;
        }

        public List<string> GetMemberNames()
        {
            return _dictionary.Keys.ToList();
        }

        public object Get(string name)
        {
            return _dictionary[name];
        }

        public void Add(string name, string value)
        {
            _dictionary[name] = value;
        }
    }
}