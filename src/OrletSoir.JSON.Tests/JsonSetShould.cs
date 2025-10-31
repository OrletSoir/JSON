using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace OrletSoir.JSON.Tests
{
    public class JsonSetShould
    {
        [Fact]
        public void ConvertToJsonCorrectly()
        {
            JsonSet set = new()
            {
                { "int", new JsonVariable(5) },
                { "bool", new JsonVariable(true) },
                { "string", new JsonVariable("foo") }
            };

            set.ToJsonString().ShouldBe(@"{""int"":5,""bool"":true,""string"":""foo""}");
        }

        [Fact]
        public void InitializeFromStringDictionary()
        {
            Dictionary<string, IJsonVariable> collection = new()
            {
                { "int", new JsonVariable(5) },
                { "bool", new JsonVariable(true) },
                { "string", new JsonVariable("foo") }
            };

            JsonSet set = new JsonSet(collection);
            VerifyCustomSet(set);
        }

        [Fact]
        public void InitializeFromJsonVariableDictionary()
        {
            Dictionary<IJsonVariable, IJsonVariable> collection = new()
            {
                { new JsonVariable("int"), new JsonVariable(5) },
                { new JsonVariable("bool"), new JsonVariable(true) },
                { new JsonVariable("string"), new JsonVariable("foo") }
            };

            JsonSet set = new JsonSet(collection);
            VerifyCustomSet(set);
        }

        private void VerifyCustomSet(JsonSet set)
        {
            set.ShouldSatisfyAllConditions(
                s => s.ShouldContainKey("int"),
                s => s["int"].ShouldSatisfyAllConditions(
                    k => k.Type.ShouldBe(JsonType.Int),
                    k => k.AsInteger().ShouldBe(5)),

                s => s.ShouldContainKey("bool"),
                s => s["bool"].ShouldSatisfyAllConditions(
                    k => k.Type.ShouldBe(JsonType.Bool),
                    k => k.AsBoolean().ShouldBeTrue()),

                s => s.ShouldContainKey("string"),
                s => s["string"].ShouldSatisfyAllConditions(
                    k => k.Type.ShouldBe(JsonType.String),
                    k => k.AsString().ShouldBe("foo")));
        }
    }
}
