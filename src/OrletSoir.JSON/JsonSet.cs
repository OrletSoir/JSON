using System;
using System.Collections.Generic;

namespace OrletSoir.JSON
{
    /// <summary>
    /// A set/dictionary/object implementation. Key can only be a <see cref="string"/> value,
    /// the Value can be any object implementing the <see cref="IJsonVariable"/> interface.<br/>
    /// Converting to Array results in losing the Key values.
    /// </summary>
    public class JsonSet : Dictionary<string, IJsonVariable>, IJsonVariable
    {
        public JsonSet()
        {
            //
        }

        public JsonSet(IEnumerable<IJsonVariable> collection)
        {
            int i = 0;
            foreach (IJsonVariable item in collection)
            {
                if (item.Type == JsonType.Tuple)
                {
                    JsonTuple jt = (JsonTuple)item;

                    Add(jt.Key.ToString(), jt.Value);
                }
                else
                {
                    while (ContainsKey(i.ToString()))
                    {
                        i++;
                    }

                    Add(i.ToString(), item);
                }
            }
        }

        public JsonSet(IEnumerable<KeyValuePair<IJsonVariable, IJsonVariable>> collection)
        {
            foreach (KeyValuePair<IJsonVariable, IJsonVariable> kvp in collection)
            {
                Add(kvp.Key.AsString(), kvp.Value);
            }
        }

        public JsonSet(IEnumerable<KeyValuePair<string, IJsonVariable>> collection)
        {
            foreach (KeyValuePair<string, IJsonVariable> kvp in collection)
            {
                Add(kvp.Key, kvp.Value);
            }
        }

        public JsonType Type => JsonType.Set;

        public string AsString()
            => throw new NotSupportedException();

        public int AsInteger()
            => throw new NotSupportedException();

        public double AsFloat()
            => throw new NotSupportedException();

        public bool AsBoolean()
            => throw new NotSupportedException();

        public DateTime? AsDateTime()
            => throw new NotSupportedException();

        public JsonArray AsArray()
            => new JsonArray(Values);

        public JsonSet AsSet()
            => this;

        public string ToJsonString()
        {
            List<string> sl = new List<string>();

            foreach (KeyValuePair<string, IJsonVariable> kvp in this)
            {
                sl.Add($"\"{kvp.Key}\":{kvp.Value.ToJsonString()}");
            }

            return $"{{{string.Join(',', sl)}}}";
        }

        public void Add(string key, int value)
            => Add(key, new JsonVariable(value));

        public void Add(string key, double value)
            => Add(key, new JsonVariable(value));

        public void Add(string key, decimal value)
            => Add(key, new JsonVariable(value));

        public void Add(string key, bool value)
            => Add(key, new JsonVariable(value));

        public void Add(string key, DateTime value)
            => Add(key, new JsonVariable(value));

        public void Add(string key, string value)
            => Add(key, new JsonVariable(value));

        public override string ToString()
            => $"{GetType().Name} (Count = {Count})";
    }
}
