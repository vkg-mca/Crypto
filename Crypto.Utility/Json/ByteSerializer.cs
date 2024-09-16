namespace Crypto.Utility.Json;

public class ByteSerializer
{
    protected static readonly ArrayPool<byte> arrayPool = ArrayPool<byte>.Shared;

    public static T Deserialize<T>(object Object)
    {
        var bytes = CovertToByteArray(Object);
        return Deserialize<T>(bytes, bytes.Length);
    }

    public static T Deserialize<T>(Stream stream)
    {
        //int contentBytesCount = 0;
        //var contentBytes = stream.ToRentedArray(arrayPool, out contentBytesCount);
        //var contentString= Encoding.UTF8.GetString(contentBytes);
        T data = default(T);
        try
        {
	 //var options = JsonSerializerOptions.Default;
	 data = JsonSerializer.Deserialize<T>(stream);
	 //data = JsonSerializer.Deserialize<T>(stream, options);
        }
        catch (Exception ex)
        {
	 throw ex;
        }

        return data;
    }

    public static T Deserialize<T>(ReadOnlySpan<byte> bytes)
    {
        // byte[] bytes = UTF8Encoding.UTF8.GetBytes(expected.ToString());

        Debug.WriteLine(typeof(T));
        Debug.WriteLine(Encoding.UTF8.GetString(bytes));
        Debug.WriteLine(typeof(T));

        try
        {
	 var options = JsonSerializerOptions.Default;

	 options = new JsonSerializerOptions()
	 {
	     NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
	     AllowTrailingCommas = true,
	     ReadCommentHandling = JsonCommentHandling.Skip,
	     UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
	 };


	 T data = JsonSerializer.Deserialize<T>(bytes, options);
	 return data;
        }
        catch (Exception ex)
        {
	 throw ex;
        }
        return default;
    }



    public static T Deserialize<T>(byte[]? bytes, int count)
    {
        // byte[] bytes = UTF8Encoding.UTF8.GetBytes(expected.ToString());

        T data = default;
        if (bytes == null || bytes.Length == 0)
	 return data;
        //var json = Encoding.UTF8.GetString(bytes, 0, count);
        //byte[] bytes1 = Encoding.UTF8.GetBytes(json);
        //var json1 = Encoding.UTF8.GetString(bytes1, 0, count);
        //var type = typeof(T);
        try
        {
	 var options = JsonSerializerOptions.Default;

	 options = new JsonSerializerOptions()
	 {
	     NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString,
	     AllowTrailingCommas = true,
	     ReadCommentHandling = JsonCommentHandling.Skip,
	     UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
	 };

	 //data = JsonSerializer.Deserialize<T>(bytes1, options);
	 data = JsonSerializer.Deserialize<T>(bytes, options);
	 //data = JsonSerializer.Deserialize<T>(bytes,0,count, JsonSerializerOptions.Default);

        }
        catch (Exception ex)
        {
	 throw ex;
        }
        return data;
    }


    public static byte[] CovertToByteArray(List<object> Objects)
    {
        var options = new JsonSerializerOptions
        {
	 PropertyNameCaseInsensitive = true,
        };
        return Objects != null && Objects.Any() ? JsonSerializer.SerializeToUtf8Bytes(Objects, options) : (new byte[] { });
    }

    public static byte[] CovertToByteArray(object Object)
    {
        var options = new JsonSerializerOptions
        {
	 PropertyNameCaseInsensitive = true,
        };
        return Object == null ? (new byte[] { }) : JsonSerializer.SerializeToUtf8Bytes(Object, options);
    }


    //public static T Deserialize<T>(object o, int abs)
    //{
    //    Stream s = new MemoryStream();
    //    byte[] bytes = ObjectToByteArray(o);
    //    s.Write(bytes, 0, bytes.Length);

    //    var o1 = UtfSerializer.Deserialize<T>(s);
    //    return o1;
    //}

    public static byte[] ObjectToByteArray(System.Object obj)
    {
        int size = Marshal.SizeOf(obj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);

        return arr;
    }
}

[ProtoContract]
public class Person
{
    [ProtoMember(1)]
    public string FirstName { get; set; }
    [ProtoMember(2)]
    public string MiddleName { get; set; }
    [ProtoMember(3)]
    public string LastName { get; set; }



}
