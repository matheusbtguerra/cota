using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Cota.Domain;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebUtilities;

namespace Cota.Infrastructure.Ana;

/// <summary>
/// A client for interacting with the ANA API to fetch telemetry data.
/// </summary>
public class AnaTelemetryClient(HttpClient http, IOptions<AnaOptions> options) : ITelemetryClient
{
    private readonly AnaOptions _opts = options.Value;
    private string? _token;
    private DateTimeOffset _tokenExpiresAt = DateTimeOffset.MinValue;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(55);

    public async Task<RiverReading?> GetLatestReadingAsync(CancellationToken ct = default)
    {
        var token = await GetValidTokenAsync(ct);
        if (token is null) return null;

        var brasilia = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow,
            TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"));
        var searchDate = brasilia.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var queryParams = new Dictionary<string, string?>
        {
            ["Código da Estação"] = KnownStations.Guaiba.Code,
            ["Tipo Filtro Data"] = "DATA_LEITURA",
            ["Data de Busca (yyyy-MM-dd)"] = searchDate,
            ["Range Intervalo de busca"] = "HORA_24"
        };

        var url = QueryHelpers.AddQueryString(
            "EstacoesTelemetricas/HidroinfoanaSerieTelemetricaAdotada/v1", queryParams);

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await http.SendAsync(request, ct);
        if (!response.IsSuccessStatusCode) return null;

        var payload = await response.Content.ReadFromJsonAsync<AnaSerieResponse>(ct);
        if (payload?.Items is null || payload.Items.Count == 0) return null;

        var latest = payload.Items
            .Where(i => i.CotaAdotadaStatus == "0" && i.CotaAdotada is not null)
            .OrderByDescending(i => ParseDate(i.DataHoraMedicao))
            .FirstOrDefault();

        if (latest is null) return null;

        if (!decimal.TryParse(latest.CotaAdotada, NumberStyles.Any,
                CultureInfo.InvariantCulture, out var cm))
            return null;

        return new RiverReading(
            LevelMeters: Math.Round(cm / 100m, 2),
            MeasuredAt: ParseDate(latest.DataHoraMedicao),
            StationName: KnownStations.Guaiba.Name);
    }

    private async Task<string?> GetValidTokenAsync(CancellationToken ct)
    {
        if (_token is not null && DateTimeOffset.UtcNow < _tokenExpiresAt)
            return _token;

        await _tokenLock.WaitAsync(ct);
        try
        {
            if (_token is not null && DateTimeOffset.UtcNow < _tokenExpiresAt)
                return _token;

            using var request = new HttpRequestMessage(
                HttpMethod.Get, "EstacoesTelemetricas/OAUth/v1");
            request.Headers.Add("Identificador", _opts.Identificador);
            request.Headers.Add("Senha", _opts.Senha);

            var response = await http.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode) return null;

            var payload = await response.Content.ReadFromJsonAsync<AnaTokenResponse>(ct);
            var token = payload?.Items?.TokenAutenticacao;
            if (string.IsNullOrEmpty(token)) return null;

            _token = token;
            _tokenExpiresAt = DateTimeOffset.UtcNow + TokenLifetime;
            return _token;
        }
        finally
        {
            _tokenLock.Release();
        }
    }

    private static DateTimeOffset ParseDate(string? raw) =>
        DateTimeOffset.TryParse(raw, CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeUniversal, out var dt)
            ? dt
            : DateTimeOffset.MinValue;
}