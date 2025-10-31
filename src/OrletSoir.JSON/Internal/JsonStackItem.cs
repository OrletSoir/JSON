namespace OrletSoir.JSON.Internal
{
    internal struct JsonStackItem
    {
        public JsonStackItemType Type;
        public IJsonVariable Value;

        public new string ToString()
        {
            switch (Type)
            {
                case JsonStackItemType.Value:
                    return Value.ToString();

                case JsonStackItemType.String:
                    return Value.AsString().ToLiteral();

                default:
                    return Type.ToString();
            }
        }
    }
}
