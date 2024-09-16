namespace Crypto.Utility.Extensions;

public static class StreamExtensions
{
    /// <summary>
    /// Copies generic stream of unknown length to rented array from provided pool.
    /// Shared array by default provides arrays up to 2^20 elements. For REST calls larger buffers are sometimes needed hence pool is a method parameter.
    /// Uses dynamic array resize to guarantee O(#bytes) complexity.
    /// </summary>
    /// <param name="stream">Input stream</param>
    /// <param name="arrayPool">Array pool</param>
    /// <param name="byteCount">Number of bytes copied from the stream</param>
    /// <param name="offset">Position of the first byte in the output array</param>
    /// <param name="copyToEndOfArray">Utf8Json only accepts byte[] of exact size or allows offset parameter to ignore number of bytes at the beginning of the array</param>
    /// <param name="predictedSize">If exact stream size is known and provided some extra byte[] to byte[] copies will be avoided</param>
    /// <param name="copyBufferSize">Allows to control copy buffer size. Default value optimized for [1k-8k] bytes range</param>
    /// <returns>Rented array THAT HAS TO BE RETURN to ArrayPool it came from</returns>
    public static byte[] ToRentedArray(this Stream stream, ArrayPool<byte> arrayPool, out int byteCount, int predictedSize = 512, int copyBufferSize = 512)
    {
        byte[] copyBuffer = null;
        byte[] contentBytes = null;
        try
        {
	 predictedSize = Math.Max(predictedSize, 64);
	 copyBufferSize = Math.Min(predictedSize, copyBufferSize);

	 //We don't want to allocate too much on the stack
	 if (copyBufferSize > 1024)
	 {
	     copyBuffer = arrayPool.Rent(copyBufferSize);
	 }

	 Span<byte> buffer = copyBuffer ?? stackalloc byte[copyBufferSize];

	 contentBytes = arrayPool.Rent(predictedSize);
	 byteCount = 0;

	 int readCount = stream.Read(buffer);
	 while (readCount != 0)
	 {
	     if (readCount + byteCount > contentBytes.Length)
	     {
	         byte[] temp = arrayPool.Rent(contentBytes.Length << 1);
	         Array.Copy(contentBytes, temp, byteCount);
	         arrayPool.Return(contentBytes);
	         contentBytes = temp;
	     }

	     buffer.Slice(0, readCount).CopyTo(contentBytes.AsSpan(byteCount, readCount));
	     byteCount += readCount;
	     readCount = stream.Read(buffer);
	 }
        }
        catch
        {
	 //When there is an exception we should return partially filled arrays back to the pool
	 if (contentBytes != null)
	 {
	     arrayPool.Return(contentBytes);
	 }

	 throw;
        }
        finally
        {
	 if (copyBuffer != null)
	 {
	     arrayPool.Return(copyBuffer);
	 }
        }

        return contentBytes;
    }
}
