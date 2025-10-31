using OrletSoir.JSON.Internal;
using System.Collections.Generic;

namespace OrletSoir.JSON
{
    /// <summary>
    /// The static JSON parser class.<br/>
    /// Parses a given JSON string into IJsonVariable object(s)
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Parse the provided JSON <see cref="string"/> into a proper JSON object, implementing <see cref="IJsonVariable"/> interface for data retrieval and/or manipulation.
        /// </summary>
        /// <param name="jsonString">Input <see cref="string"/>.</param>
        /// <returns>Parsed object implementing the <see cref="IJsonVariable"/> interface.</returns>
        public static IJsonVariable Parse(string jsonString)
        {
            Queue<JsonStackItem> tokenStack = Tokenizer.ProcessJson(jsonString);
            return TokenProcessor.ProcessStack(tokenStack);
        }
    }
}
