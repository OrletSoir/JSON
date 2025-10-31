using AutoFixture.Xunit2;
using Shouldly;
using System;
using Xunit;

namespace OrletSoir.JSON.Tests
{
    public class JsonVariableShould
    {
        [Theory, AutoData]
        public void SerializeFromInteger(int someInteger)
        {
            JsonVariable value = new JsonVariable(someInteger);

            value.AsInteger().ShouldBe(someInteger);
            value.AsFloat().ShouldBe(someInteger);
            value.AsDateTime().ShouldBe(Tools.UnixTimestampToDate((uint)someInteger));
            value.AsBoolean().ShouldBe(someInteger > 0);
            value.AsString().ShouldBe(someInteger.ToString());

            value.AsArray().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Array),
                i => i.ShouldContain(v => v.Type == JsonType.Int && v.AsInteger() == someInteger));

            value.AsSet().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Set),
                i => i.ShouldContain(s => s.Key == "Int" && value.Type == JsonType.Int && value.AsInteger() == someInteger));
        }

        [Theory, AutoData]
        public void SerializeFromFloat(double someFloat)
        {
            JsonVariable value = new JsonVariable(someFloat);

            value.AsInteger().ShouldBe((int)someFloat);
            value.AsFloat().ShouldBe(someFloat);
            value.AsDateTime().ShouldBe(Tools.UnixTimestampToDate((uint)someFloat));
            value.AsBoolean().ShouldBe(someFloat > 0);
            value.AsString().ShouldBe(someFloat.ToString());

            value.AsArray().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Array),
                i => i.ShouldContain(v => v.Type == JsonType.Float && v.AsFloat() == someFloat));

            value.AsSet().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Set),
                i => i.ShouldContain(s => s.Key == "Float" && value.Type == JsonType.Float && value.AsFloat() == someFloat));
        }

        [Theory, AutoData]
        public void SerializeFromDecimal(decimal someDecimal)
        {
            JsonVariable value = new JsonVariable(someDecimal);

            value.AsInteger().ShouldBe((int)someDecimal);
            value.AsFloat().ShouldBe((double)someDecimal);
            value.AsDateTime().ShouldBe(Tools.UnixTimestampToDate((uint)someDecimal));
            value.AsBoolean().ShouldBe(someDecimal > 0);
            value.AsString().ShouldBe(someDecimal.ToString());

            value.AsArray().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Array),
                i => i.ShouldContain(v => v.Type == JsonType.Float && v.AsFloat() == (double)someDecimal));

            value.AsSet().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Set),
                i => i.ShouldContain(s => s.Key == "Float" && value.Type == JsonType.Float && value.AsFloat() == (double)someDecimal));
        }

        [Theory, AutoData]
        public void SerializeFromDateTime(DateTime someDate)
        {
            JsonVariable value = new JsonVariable(someDate);

            value.AsInteger().ShouldBe((int)Tools.ToUnixTimestamp(someDate));
            value.AsFloat().ShouldBe(Tools.ToUnixTimestamp(someDate));
            value.AsDateTime().ShouldBe(someDate);
            value.AsBoolean().ShouldBeTrue();
            value.AsString().ShouldBe(someDate.ToString("yyyy-MM-dd HH':'mm':'ss"));

            value.AsArray().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Array),
                i => i.ShouldContain(v => v.Type == JsonType.DateTime && v.AsDateTime() == someDate));

            value.AsSet().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Set),
                i => i.ShouldContain(s => s.Key == "DateTime" && value.Type == JsonType.DateTime && value.AsDateTime() == someDate));
        }

        [Theory]
        [InlineData(false, "false", 0, 0.0)]
        [InlineData(true, "true", 1, 1.0)]
        public void SerializeFromBool(bool someBool, string expectedString, int expectedInt, double expectedFloat)
        {
            JsonVariable value = new JsonVariable(someBool);

            value.AsInteger().ShouldBe(expectedInt);
            value.AsFloat().ShouldBe(expectedFloat);
            value.AsDateTime().ShouldBeNull();
            value.AsBoolean().ShouldBe(someBool);
            value.AsString().ShouldBe(expectedString);

            value.AsArray().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Array),
                i => i.ShouldContain(v => v.Type == JsonType.Bool && v.AsBoolean() == someBool));

            value.AsSet().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Set),
                i => i.ShouldContain(s => s.Key == "Bool" && value.Type == JsonType.Bool && value.AsBoolean() == someBool));
        }

        [Theory, InlineData("2025-10-10 12:34:56")]
        public void SerializeFromString(string someString)
        {
            JsonVariable value = new JsonVariable(someString);

            value.AsInteger().ShouldBe(Tools.ToInt(someString));
            value.AsFloat().ShouldBe(Tools.ToDouble(someString));
            value.AsDateTime().ShouldBe(Tools.ToDateTime(someString, DateTime.Now));
            value.AsBoolean().ShouldBe(Tools.ToInt(someString) > 0);
            value.AsString().ShouldBe(someString);

            value.AsArray().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Array),
                i => i.ShouldContain(v => v.Type == JsonType.String && v.AsString() == someString));

            value.AsSet().ShouldSatisfyAllConditions(
                i => i.Type.ShouldBe(JsonType.Set),
                i => i.ShouldContain(s => s.Key == "String" && value.Type == JsonType.String && value.AsString() == someString));
        }

        [Theory]
        [InlineData("test-no-escapes", @"""test-no-escapes""")]
        [InlineData("test-with-\"quotes\"", @"""test-with-\""quotes\""""")]
        [InlineData("C:\\Windows\\Path\\", @"""C:\\Windows\\Path\\""")]
        public void EscapeStringMagicCharacters(string input, string expected)
        {
            JsonVariable variable = new JsonVariable(input);

            string result = variable.ToJsonString();

            result.ShouldBe(expected);
        }
    }
}
