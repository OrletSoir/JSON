using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace OrletSoir.JSON.Tests
{
    public class JsonShould
    {
        [Fact]
        public void ParseSimpleJsonObject()
        {
            IJsonVariable result = Json.Parse(@"{ ""foo"": ""bar"", ""baz"": ""quux"" }");

            result.Type.ShouldBe(JsonType.Set);

            Action asSet = () => result.AsSet();
            asSet.ShouldNotThrow();

            JsonSet resultSet = result.AsSet();

            resultSet.ShouldContainKey("foo");
            resultSet["foo"].Type.ShouldBe(JsonType.String);
            resultSet["foo"].AsString().ShouldBe("bar");

            resultSet.ShouldContainKey("baz");
            resultSet["baz"].Type.ShouldBe(JsonType.String);
            resultSet["baz"].AsString().ShouldBe("quux");
        }

        [Fact]
        public void ParseSimpleJsonOArray()
        {
            int[] expectedValues = new int[] { 1, 2, 3, 4, 5 };
            IJsonVariable result = Json.Parse(@"[1, 2, 3, 4, 5]");

            result.Type.ShouldBe(JsonType.Array);

            Action asArray = () => result.AsArray();
            asArray.ShouldNotThrow();

            JsonArray resultArray = result.AsArray();

            resultArray.Count.ShouldBe(5);

            resultArray.ShouldAllBe(jv => jv.Type == JsonType.Int);

            int[] values = resultArray.Select(i => i.AsInteger()).ToArray();
            values.ShouldBe(expectedValues);
        }

        [Fact]
        public void ParseMixedJsonObject()
        {
            IJsonVariable result = Json.Parse(@"{ ""string"" : ""foo"", ""int"" : 42, ""float"" : 3.14, ""array"" : [1, 2, 3], ""object"" : { ""foo"": ""bar"" }, bool : true }");

            result.Type.ShouldBe(JsonType.Set);

            JsonSet resultSet = result.AsSet();

            resultSet["string"].Type.ShouldBe(JsonType.String);
            resultSet["int"].Type.ShouldBe(JsonType.Int);
            resultSet["float"].Type.ShouldBe(JsonType.Float);
            resultSet["array"].Type.ShouldBe(JsonType.Array);
            resultSet["object"].Type.ShouldBe(JsonType.Set);
            resultSet["bool"].Type.ShouldBe(JsonType.Bool);
        }
    }
}
