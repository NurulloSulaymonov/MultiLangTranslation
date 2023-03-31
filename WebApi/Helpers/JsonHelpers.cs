using Infrastructure.Data.Entities;

namespace WebApi.Helpers;

public static class JsonHelpers
{
    public static string ToJson(this object obj)
    {
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
            WriteIndented = true,
        };
        return System.Text.Json.JsonSerializer.Serialize(obj, options);
    }
    
public static T FromJson<T>(this string json)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(json);
    }


}