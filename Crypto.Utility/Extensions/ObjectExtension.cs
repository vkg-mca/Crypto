using Newtonsoft.Json;

namespace Crypto.Utility.Extensions;

public static class ObjectExtension
{
    public static string ToJsonString(this object instance)
    {
        return JsonConvert.SerializeObject(instance, Formatting.Indented);
    }
}
