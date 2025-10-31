using System.Collections.Generic;

namespace OrletSoir.JSON.Internal
{
    internal class TokenProcessor
    {
        public static IJsonVariable ProcessStack(Queue<JsonStackItem> tokenStack)
        {
            Stack<JsonStackItem> jsonStack = new Stack<JsonStackItem>();
            Stack<int> tupleDepth = new Stack<int>();
            int depth = 0;

            while (tokenStack.Count > 0)
            {
                JsonStackItem currentToken = tokenStack.Dequeue();

                switch (currentToken.Type)
                {
                    // if it's a value, push it into the jsonStack
                    case JsonStackItemType.Value:
                    case JsonStackItemType.String:
                        jsonStack.Push(currentToken);
                        break;

                    // if it's an open array/set marker, push it into the jsonStack and increment depth counter so we won't accidentally a tuple
                    case JsonStackItemType.OpenArrayMarker:
                    case JsonStackItemType.OpenSetMarker:
                        jsonStack.Push(currentToken);
                        depth++;
                        break;

                    // if it's a close array marker, pop all the items from jsonStack into a temporary stack until first open array marker, then create an array of popped values
                    // (temp stack is required to maintain the original item sequencing)
                    case JsonStackItemType.CloseArrayMarker:
                        ProcessArrayCloseMarker(jsonStack);

                        // decrement the depth counter
                        depth--;
                        break;

                    // if it's a close set marker, pop all the items from jsonStack into a temporary stack until first open set marker, then create a set of popped value tuples
                    // (temp stack is required to maintain the original item sequencing)
                    case JsonStackItemType.CloseSetMarker:
                        ProcessSetCloseMarker(jsonStack);

                        // decrement the depth counter
                        depth--;
                        break;
                }

                // if tupleDepth top element points at current tuple level, pop two items from jsonStack and combine them into a tuple, then push the tuple back into jsonStack;
                if (tupleDepth.Count > 0 && tupleDepth.Peek() == depth)
                {
                    CollapseTuple(jsonStack);

                    // remove the tuple item for current depth from tuple stack
                    tupleDepth.Pop();
                }

                // push current tuple depth into tuple stack
                if (currentToken.Type == JsonStackItemType.TupleMarker)
                {
                    tupleDepth.Push(depth);
                }
            }

            if (jsonStack.Count > 0)
            {
                return jsonStack.Pop().Value;
            }

            return new JsonVariable();
        }

        private static void ProcessArrayCloseMarker(Stack<JsonStackItem> jsonStack)
        {
            Stack<JsonStackItem> tempStack = new Stack<JsonStackItem>();

            // put items into new stack
            JsonStackItem token = jsonStack.Pop();
            while (token.Type == JsonStackItemType.Value || token.Type == JsonStackItemType.String)
            {
                tempStack.Push(token);
                token = jsonStack.Pop();
            }

            // error check
            if (token.Type != JsonStackItemType.OpenArrayMarker)
            {
                throw new JsonParseException($"Unexpected stack item! Expected `{JsonStackItemType.OpenArrayMarker}` got `{token.Type}`.");
            }

            // create the array
            JsonArray ja = new JsonArray();
            while (tempStack.Count > 0)
            {
                ja.Add(tempStack.Pop().Value);
            }

            // push the value back into jsonStack
            jsonStack.Push(new JsonStackItem { Type = JsonStackItemType.Value, Value = ja });
        }

        private static void ProcessSetCloseMarker(Stack<JsonStackItem> jsonStack)
        {
            Stack<JsonStackItem> tempStack = new Stack<JsonStackItem>();

            // pup items into new stack
            JsonStackItem token = jsonStack.Pop();
            while (token.Type == JsonStackItemType.Value || token.Type == JsonStackItemType.String)
            {
                tempStack.Push(token);
                token = jsonStack.Pop();
            }

            // error check
            if (token.Type != JsonStackItemType.OpenSetMarker)
            {
                throw new JsonParseException($"Unexpected stack item! Expected `{JsonStackItemType.OpenSetMarker}` got `{token.Type}`.");
            }

            // push the value back into jsonStack
            jsonStack.Push(new JsonStackItem { Type = JsonStackItemType.Value, Value = CollapseSet(tempStack) });
        }

        private static void CollapseTuple(Stack<JsonStackItem> jsonStack)
        {
            IJsonVariable value = jsonStack.Pop().Value;
            IJsonVariable key = jsonStack.Pop().Value;

            jsonStack.Push(new JsonStackItem { Type = JsonStackItemType.Value, Value = new JsonTuple(key, value) });
        }

        private static JsonSet CollapseSet(Stack<JsonStackItem> tempStack)
        {
            JsonSet newSet = new JsonSet();

            while (tempStack.Count > 0)
            {
                JsonStackItem item = tempStack.Pop();
                if (item.Value.Type == JsonType.Tuple)
                {
                    JsonTuple tuple = (JsonTuple)item.Value;

                    newSet.Add(tuple.Key.ToString(), tuple.Value);
                }
            }

            return newSet;
        }
    }
}
