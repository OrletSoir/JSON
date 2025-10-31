using Shouldly;
using Xunit;

namespace OrletSoir.JSON.Tests
{
    public class JsonArrayShould
    {
        [Fact]
        public void ConvertToJsonCorrectly()
        {
            JsonArray array = new()
            {
                new JsonVariable(5),
                new JsonVariable(true),
                new JsonVariable("foo")
            };

            array.ToJsonString().ShouldBe(@"[5,true,""foo""]");
        }

        [Fact]
        public void InitializeFromJsonVariableCollection()
        {
            IJsonVariable[] collection = new IJsonVariable[]
            {
                new JsonVariable(5),
                new JsonVariable(true),
                new JsonVariable("foo")
            };

            JsonArray array = new JsonArray(collection);

            array.ShouldSatisfyAllConditions(
                s => s.Count.ShouldBe(3),

                s => s[0].ShouldSatisfyAllConditions(
                    k => k.Type.ShouldBe(JsonType.Int),
                    k => k.AsInteger().ShouldBe(5)),

                s => s[1].ShouldSatisfyAllConditions(
                    k => k.Type.ShouldBe(JsonType.Bool),
                    k => k.AsBoolean().ShouldBeTrue()),

                s => s[2].ShouldSatisfyAllConditions(
                    k => k.Type.ShouldBe(JsonType.String),
                    k => k.AsString().ShouldBe("foo")));
        }
    }
}
