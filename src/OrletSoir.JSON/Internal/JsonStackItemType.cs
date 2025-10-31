namespace OrletSoir.JSON.Internal
{
    internal enum JsonStackItemType
    {
        OpenSetMarker,
        CloseSetMarker,
        OpenArrayMarker,
        CloseArrayMarker,
        TupleMarker,
        ItemDelimiter,
        Value,
        String
    }
}
