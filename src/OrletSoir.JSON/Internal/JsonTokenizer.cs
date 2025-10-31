using System.Collections.Generic;

namespace OrletSoir.JSON.Internal
{
    internal static class Tokenizer
    {
        /**
		 * Parsing tokens and analysis:
		 * { - set start
		 * } - set end
		 * : - key-value delimiter in set
		 * , - item delimiter in set/array
		 * " - string delimiter (start then end)
		 * \ - escape character for next symbol (in-string only, regular unescaping rules)
		 * [ - array start
		 * ] - array end
		 */

        /// <summary>
        /// Walks the string and parses it into queue of stack items for further processing.<br/>
		/// Converts values into variables and puts them into the queue as well.
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns>A <see cref="Queue{T}"/> of <see cref="JsonStackItem"/> for further processing.</returns>
        public static Queue<JsonStackItem> ProcessJson(string jsonString)
        {
            List<char> whiteSpace = new List<char> { ' ', '\n', '\r', '\t', '\0' };
            Queue<JsonStackItem> tokenQueue = new Queue<JsonStackItem>();

            bool valueCollectingMode = false;
            int valueStartIndex = 0;

            int index = 0;
            while (index < jsonString.Length)
            {
                switch (jsonString[index])
                {
                    /* array and set tokens */
                    case JsonToken.OpenSet:
                        // if we're collecting value, add it first
                        if (valueCollectingMode)
                        {
                            tokenQueue.Enqueue(CreateValue(jsonString, valueStartIndex, index));
                            valueCollectingMode = false;
                        }

                        tokenQueue.Enqueue(new JsonStackItem { Type = JsonStackItemType.OpenSetMarker });
                        break;

                    case JsonToken.CloseSet:
                        // if we're collecting value, add it first
                        if (valueCollectingMode)
                        {
                            tokenQueue.Enqueue(CreateValue(jsonString, valueStartIndex, index));
                            valueCollectingMode = false;
                        }

                        tokenQueue.Enqueue(new JsonStackItem { Type = JsonStackItemType.CloseSetMarker });
                        break;

                    case JsonToken.OpenArray:
                        // if we're collecting value, add it first
                        if (valueCollectingMode)
                        {
                            tokenQueue.Enqueue(CreateValue(jsonString, valueStartIndex, index));
                            valueCollectingMode = false;
                        }

                        tokenQueue.Enqueue(new JsonStackItem { Type = JsonStackItemType.OpenArrayMarker });
                        break;

                    case JsonToken.CloseArray:
                        // if we're collecting value, add it first
                        if (valueCollectingMode)
                        {
                            tokenQueue.Enqueue(CreateValue(jsonString, valueStartIndex, index));
                            valueCollectingMode = false;
                        }

                        tokenQueue.Enqueue(new JsonStackItem { Type = JsonStackItemType.CloseArrayMarker });
                        break;

                    case JsonToken.KeyValueSeparator:
                        // if we're collecting value, add it first
                        if (valueCollectingMode)
                        {
                            tokenQueue.Enqueue(CreateValue(jsonString, valueStartIndex, index));
                            valueCollectingMode = false;
                        }

                        tokenQueue.Enqueue(new JsonStackItem { Type = JsonStackItemType.TupleMarker });
                        break;

                    case JsonToken.ItemDelimeter:
                        // if we're collecting value, add it first
                        if (valueCollectingMode)
                        {
                            tokenQueue.Enqueue(CreateValue(jsonString, valueStartIndex, index));
                            valueCollectingMode = false;
                        }
                        break;

                    case JsonToken.StringDelimiter:
                        // if we're collecting value, drop it
                        if (valueCollectingMode)
                        {
                            valueCollectingMode = false;
                        }

                        string value = ExtractStringValue(jsonString, index + 1);
                        tokenQueue.Enqueue(CreateString(value));
                        index += value.Length + 1;
                        break;

                    default:
                        if (whiteSpace.Contains(jsonString[index]))
                        {
                            break;
                        }

                        if (!valueCollectingMode)
                        {
                            valueStartIndex = index;
                        }

                        valueCollectingMode = true;
                        break;
                }

                index++;
            }

            if (valueCollectingMode)
            {
                tokenQueue.Enqueue(new JsonStackItem { Type = JsonStackItemType.Value, Value = JsonVariable.FromString(jsonString.Substring(valueStartIndex, index - valueStartIndex), JsonStackItemType.Value) });
            }

            return tokenQueue;
        }

        /**
		 * Extract string value starting from position till first unescaped string separator literal.
		 * Also unescape any escaped characters found there.
		 */
        private static string ExtractStringValue(string jsonString, int startIndex)
        {
            bool prevEsc = false; // flag if previous character was the escape character

            for (int i = startIndex; i < jsonString.Length; i++)
            {
                if (jsonString[i] == JsonToken.StringDelimiter && !prevEsc)
                {
                    //return System.Text.RegularExpressions.Regex.Unescape(jsonString.Substring(startIndex, i - startIndex));
                    //return Microsoft.JScript.GlobalObject.unescape(jsonString.Substring(startIndex, i - startIndex));
                    return jsonString.Substring(startIndex, i - startIndex);
                }

                prevEsc = jsonString[i] == JsonToken.Escape;
            }

            return jsonString.Substring(startIndex);
        }

        private static JsonStackItem CreateValue(string jsonString, int startIndex, int endIndex)
            => new JsonStackItem { Type = JsonStackItemType.Value, Value = JsonVariable.FromString(jsonString.Substring(startIndex, endIndex - startIndex), JsonStackItemType.Value) };

        private static JsonStackItem CreateString(string value)
            => new JsonStackItem { Type = JsonStackItemType.String, Value = JsonVariable.FromString(value.Unescape(), JsonStackItemType.String) };
    }
}
