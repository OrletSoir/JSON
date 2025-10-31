using System;

namespace OrletSoir.JSON
{
    /// <summary>
    /// Interface for accessing the value in various forms.
    /// </summary>
    public interface IJsonVariable
    {
        /// <summary>
        /// Describes the underlying type.
        /// </summary>
        JsonType Type { get; }

        /// <summary>
        /// Returns the value as a <see cref="string"/>.<br/>
        /// If the underlying value is not <see cref="JsonType.String"/>, then the value will be converted to <see cref="string"/> to the best ability of the implementor.
        /// </summary>
        /// <returns><see cref="string"/> representation of the underlying value.</returns>
        string AsString();

        /// <summary>
        /// Returns the value as an <see cref="int"/>.<br/>
        /// If the underlying value is not <see cref="JsonType.Int"/>, then the value will be converted to <see cref="int"/> to the best ability of the implementor.
        /// </summary>
        /// <returns><see cref="int"/> representation of the underlying value.</returns>
        int AsInteger();

        /// <summary>
        /// Returns the value as a <see cref="double"/>.<br/>
        /// If the underlying value is not <see cref="JsonType.Float"/>, then the value will be converted to <see cref="double"/> to the best ability of the implementor.
        /// </summary>
        /// <returns><see cref="double"/> representation of the underlying value.</returns>
        double AsFloat();

        /// <summary>
        /// Returns the value as a <see cref="bool"/>.<br/>
        /// If the underlying value is not <see cref="JsonType.Bool"/>, then the value will be converted to <see cref="bool"/> to the best ability of the implementor.
        /// </summary>
        /// <returns><see cref="bool"/> representation of the underlying value.</returns>
        bool AsBoolean();

        /// <summary>
        /// Returns the value as a <see cref="DateTime"/>.<br/>
        /// If the underlying value is not <see cref="JsonType.DateTime"/>, then the value will be converted to <see cref="DateTime"/> to the best ability of the implementor.
        /// </summary>
        /// <returns><see cref="string"/> representation of the underlying value.</returns>
        DateTime? AsDateTime();

        /// <summary>
        /// Returns the value as a <see cref="JsonArray"/>.<br/>
        /// If the underlying value is not <see cref="JsonType.Array"/>, then the value will be converted to <see cref="JsonArray"/> to the best ability of the implementor.
        /// </summary>
        /// <returns><see cref="string"/> representation of the underlying value.</returns>
        JsonArray AsArray();

        /// <summary>
        /// Returns the value as a <see cref="JsonSet"/>.<br/>
        /// If the underlying value is not <see cref="JsonType.Set"/>, then the value will be converted to <see cref="JsonSet"/> to the best ability of the implementor.
        /// </summary>
        /// <returns><see cref="string"/> representation of the underlying value.</returns>
        JsonSet AsSet();

        /// <summary>
        /// Serializes the underlying value into a proper JSON representation of its value.
        /// </summary>
        /// <returns>JSON <see cref="string"/> of the underlying value.</returns>
        string ToJsonString();
    }
}
