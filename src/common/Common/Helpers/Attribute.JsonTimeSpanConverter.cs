namespace AsteriskDotHMG.Common.Helpers;

public class JsonTimeSpanConverter : STJ.JsonConverter<TimeSpan>
{
    public bool WithoutSeconds { get; init; }

    public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var input = reader.GetString();

        if (string.IsNullOrWhiteSpace(input))
        {
            return default;
        }

        return TimeSpan.Parse(input);
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(
            WithoutSeconds
                ? $"{value.Hours:D2}:{value.Minutes:D2}"
                : value.ToString("c"));
    }
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class JsonTimeSpanConverterAttribute : STJ.JsonConverterAttribute
{
    public bool WithoutSeconds { get; init; }

    public override STJ.JsonConverter CreateConverter(Type typeToConvert)
    {
        return new JsonTimeSpanConverter
        {
            WithoutSeconds = WithoutSeconds
        };
    }
}
