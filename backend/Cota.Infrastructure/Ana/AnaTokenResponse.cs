using System.Text.Json.Serialization;

namespace Cota.Infrastructure.Ana;

internal sealed record AnaTokenResponse(
    [property: JsonPropertyName("items")] AnaTokenItems? Items);

internal sealed record AnaTokenItems(
    [property: JsonPropertyName("tokenautenticacao")] string? TokenAutenticacao);