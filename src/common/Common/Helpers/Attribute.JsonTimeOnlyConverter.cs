namespace AsteriskDotHMG.Common.Helpers;

public class JsonTimeOnlyConverter : STJ.JsonConverter<TimeOnly>
{
    public bool WithoutSeconds { get; init; }

    public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var input = reader.GetString();

        if (string.IsNullOrWhiteSpace(input))
        {
            return default;
        }

        return TimeOnly.Parse(input);
    }

    public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(
            value.ToString(WithoutSeconds ? "HH:mm" : "HH:mm:ss")
        );
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class JsonTimeOnlyConverterAttribute : STJ.JsonConverterAttribute
{
    public bool WithoutSeconds { get; init; }

    public override STJ.JsonConverter CreateConverter(Type typeToConvert)
    {
        return new JsonTimeOnlyConverter
        {
            WithoutSeconds = WithoutSeconds
        };
    }
}
