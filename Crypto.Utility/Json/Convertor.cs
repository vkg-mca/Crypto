namespace Crypto.Utility.Json;

public class DecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        try
        {
	 if (reader.TokenType == JsonTokenType.String)
	 {
	     ReadOnlySpan<byte> span = reader.HasValueSequence
	         ? reader.ValueSequence.ToArray()
	         : reader.ValueSpan;
	     if (Utf8Parser.TryParse(span, out decimal number, out int bytesConsumed)
	         && span.Length == bytesConsumed)
	     {
	         return number;
	     }

	     if (decimal.TryParse(reader.GetString(), out number))
	     {
	         return number;
	     }
	 }

	 return reader.GetDecimal();
        }
        catch (Exception ex)
        {

	 var msg = ex.Message;
        }
        return default;
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class DoubleConverter : JsonConverter<double>
{
    public override double Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        try
        {
	 if (reader.TokenType == JsonTokenType.String)
	 {
	     ReadOnlySpan<byte> span = reader.HasValueSequence
	         ? reader.ValueSequence.ToArray()
	         : reader.ValueSpan;
	     if (Utf8Parser.TryParse(span, out double number, out int bytesConsumed)
	         && span.Length == bytesConsumed)
	     {
	         return number;
	     }

	     if (double.TryParse(reader.GetString(), out number))
	     {
	         return number;
	     }
	 }

	 return reader.GetDouble();
        }
        catch (Exception ex)
        {

	 var msg = ex.Message;
        }
        return double.NaN;
    }

    public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class IntConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
	 ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
	 if (Utf8Parser.TryParse(span, out int number, out int bytesConsumed) && span.Length == bytesConsumed)
	 {
	     return number;
	 }

	 if (int.TryParse(reader.GetString(), out number))
	 {
	     return number;
	 }
        }

        return reader.GetInt32();
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}