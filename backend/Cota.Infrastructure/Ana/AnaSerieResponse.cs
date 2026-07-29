using System.Text.Json.Serialization;

namespace Cota.Infrastructure.Ana;

internal sealed record AnaSerieResponse(
    [property: JsonPropertyName("status")] string Status,
    [property: JsonPropertyName("code")] int Code,
    [property: JsonPropertyName("items")] List<AnaSerieItem>? Items);

internal sealed record AnaSerieItem(
    [property: JsonPropertyName("Cota_Adotada")] string? CotaAdotada,
    [property: JsonPropertyName("Cota_Adotada_Status")] string? CotaAdotadaStatus,
    [property: JsonPropertyName("Data_Hora_Medicao")] string? DataHoraMedicao);