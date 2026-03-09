using System.Text.Json;
using Microsoft.Extensions.Logging;
#if HAS_NEWTONSOFT
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
#endif

namespace ImSharp;

#if HAS_NEWTONSOFT
/// <summary> Conversion from and to string. </summary>
public class InlineStringU8ConverterNewtonSoft : JsonConverter
{
    /// <inheritdoc/>
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        => writer.WriteValue(value?.ToString());

    /// <inheritdoc/>
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        var text = serializer.Deserialize<string>(reader);
        if (text is null)
            return existingValue ?? Activator.CreateInstance(objectType);

        return Activator.CreateInstance(objectType, text);
    }

    /// <inheritdoc/>
    public override bool CanConvert(Type objectType)
        => objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(InlineStringU8<>);
}
#endif

/// <summary> Conversion from and to string. </summary>
public class InlineStringU8Converter<TBacking> : System.Text.Json.Serialization.JsonConverter<InlineStringU8<TBacking>>
    where TBacking : unmanaged, IBinaryInteger<TBacking>
{
    /// <inheritdoc/>
    public override InlineStringU8<TBacking> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var arrayPool      = ImSharpConfiguration.ArrayPool;
        var lengthEstimate = reader.ValueSpan.Length;
        var buffer         = arrayPool.Rent(lengthEstimate is 0 ? ImSharpConfiguration.ArrayPoolRequestSizeSmall : lengthEstimate);
        try
        {
            var length = reader.CopyString(buffer);
            return new InlineStringU8<TBacking>(buffer.AsSpan(0, length));
        }
        finally
        {
            arrayPool.Return(buffer);
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, InlineStringU8<TBacking> value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.GetBytes());
}

internal class InlineStringU8JsonConverterAttribute : System.Text.Json.Serialization.JsonConverterAttribute
{
    public override System.Text.Json.Serialization.JsonConverter? CreateConverter(Type typeToConvert)
        => (System.Text.Json.Serialization.JsonConverter?)Activator.CreateInstance(
            typeof(InlineStringU8Converter<>).MakeGenericType(typeToConvert.GetGenericArguments()));
}
