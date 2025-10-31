namespace OrletSoir.JSON.Internal
{
    internal struct JsonToken
	{
		/**
		 * Open and Close set tokens.
		 * Create a JsonSet from objects in the stack starting by last OpenSet token.
		 */
		public const char OpenSet = '{';
		public const char CloseSet = '}';

		/**
		 * Key-Value separator.
		 * Specifies that during a new object creation two values should be popped and made into a KVP.
		 */
		public const char KeyValueSeparator = ':';

		/**
		 * Open and close array tokens.
		 * Create a JsonArray from objects in the stack starting by last OpenArray token.
		 */
		public const char OpenArray = '[';
		public const char CloseArray = ']';

		/**
		 * Item delimeter.
		 * Pushes current value into the stack.
		 */
		public const char ItemDelimeter = ',';

		/**
		 * Escape character.
		 * Escapes the following literal to create a specific symbol.
		 * Used only within strings.
		 */
		public const char Escape = '\\';

		/**
		 * String delimiter.
		 * Returns a string starting from next symbol and ending at first non-escaped string delimiter.
		 */
		public const char StringDelimiter = '"';
	}
}
