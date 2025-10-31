using System;
using System.Collections.Generic;

namespace OrletSoir.JSON
{
    /// <summary>
    /// Array type. Is an array of objects implementing the<see cref="IJsonVariable"/> interface.<br/>
    /// Converting to Set will make Keys out of indices.
    /// </summary>
    public class JsonArray : List<IJsonVariable>, IJsonVariable
    {
        public JsonArray()
        {
            //
        }

        public JsonArray(IEnumerable<IJsonVariable> collection)
        {
            AddRange(collection);
        }

        public JsonType Type => JsonType.Array;

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
            => this;

        public JsonSet AsSet()
        {
            JsonSet ns = new JsonSet();

            for (int i = 0; i < Count; i++)
            {
                ns.Add(i.ToString(), this[i]);
            }

            return ns;
        }

        public string ToJsonString()
        {
            List<string> sl = new List<string>();

            foreach (IJsonVariable jval in this)
            {
                sl.Add(jval.ToJsonString());
            }

            return $"[{string.Join(',', sl)}]";
        }

        public override string ToString()
            => $"{GetType().Name} (Count = {Count})";
    }
}
