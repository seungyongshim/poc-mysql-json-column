using System.Reflection;
using System.Text.Json;

namespace TypedJson1;

public static class TypedJson
{
    public static string Serialize<T>(T msg) where T : notnull
    {
        var quailty = msg.GetType().AssemblyQualifiedName;
        var assembly = msg.GetType().Assembly.GetName().Name;
        var type = msg.GetType().FullName!;

        return @$"{{""A"":""{assembly}"",""T"":""{type}"",""V"":{JsonSerializer.Serialize(msg, new JsonSerializerOptions
        {

        })}}}";
    }

    public static object? Deserialize(string jsonString)
    {
        var root = JsonDocument.Parse(jsonString).RootElement;

        var assembly = root.GetProperty("A").GetString();
        var typeName = root.GetProperty("T").GetString();

        var type = Assembly.Load(assembly!).GetType(typeName!);

        return root.GetProperty("V").Deserialize(type!, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}
