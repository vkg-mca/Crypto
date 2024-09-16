using Crypto.Utility.Json;
using FluentAssertions;

namespace Crypto.MarketData.UnitTest;

public class ExtenssionsTest
{
    [Fact]
    public void Deserialize_BytesAndCount_Object()
    {
        var expected = new Person { FirstName = "Vinod", MiddleName = "Kumar", LastName = "Gupta" };
        var actual = ByteSerializer.Deserialize<Person>(expected);
        expected.Should().BeEquivalentTo(actual);
    }

    //[Fact]
    //public void DeserializeByProto_BytesAndCount_Object()
    //{
    //    var expected = new Person { FirstName = "Vinod", MiddleName = "Kumar", LastName = "Gupta" };
    //    var actual = UtfSerializer.Deserialize<Person>(expected, 0);
    //    expected.Should().BeEquivalentTo(actual);
    //}

}




