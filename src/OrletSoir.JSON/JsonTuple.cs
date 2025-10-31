using System;

namespace OrletSoir.JSON
{
    /// <summary>
    /// A tuple of two objects implementing the IJsonVariable interface. One acting as a <see cref="Key"/>, other as <see cref="Value"/>.<br/>
    /// Returns <see cref="Value"/> by default.
    /// </summary>
    public class JsonTuple : IJsonVariable
    {
        public IJsonVariable Key { get; private set; }
        public IJsonVariable Value { get; private set; }

        public JsonTuple(IJsonVariable key, IJsonVariable value)
        {
            Key = key;
            Value = value;
        }

        public JsonType Type => JsonType.Tuple;

        public string AsString()
            => Value.AsString();

        public int AsInteger()
            => Value.AsInteger();

        public double AsFloat()
            => Value.AsFloat();

        public bool AsBoolean()
            => Value.AsBoolean();

        public DateTime? AsDateTime()
            => Value.AsDateTime();

        public JsonArray AsArray()
            => new JsonArray { Key, Value };

        public JsonSet AsSet()
            => new JsonSet { { Key.AsString(), Value } };

        public string ToJsonString()
            => string.Format("{0}:{1}", Key.ToJsonString(), Value.ToJsonString());

        public override string ToString()
            => string.Format("{0} ({1}, {2})", GetType().Name, Key, Value);
    }
}
