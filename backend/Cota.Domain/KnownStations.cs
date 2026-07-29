namespace Cota.Domain;

public class KnownStations
{
    /// <summary>
    /// Usina do Gasômetro station (ANA code 87450020), which monitors the Guaíba
    /// river level in Porto Alegre. It replaced the Cais Mauá C6 station (87450004),
    /// damaged in the May 2024 flood.
    /// </summary>
    /// <remarks>
    /// Reference levels follow the telemetric series adjusted to mean sea level
    /// (Imbituba tide gauge), which is the reference this station returns through the API.
    /// WARNING: the station's physical staff gauge uses its own thresholds (alert 3.15 m /
    /// flood 3.60 m), which do NOT match the value read from telemetry — using those here
    /// would make the app underestimate the risk. The values below match the API data.
    ///
    /// - Flood 2.60 m: confirmed by multiple sources (2026) for station 87450020.
    /// - Alert 2.55 m: alert threshold in the current reference.
    /// - Attention 2.20 m: product margin (not an official threshold) to warn before alert.
    ///
    /// PENDING: confirm alert and attention against the primary source (the API's own
    /// HidroInventarioEstacoes route, or the station's official record on ANA's HidroWeb portal).
    ///
    /// Sources:
    /// - Flood 2.60 m (station 87450020): https://www.jornaldocomercio.com/geral/2026/07/1257573-guaiba-esta-abaixo-da-cota-de-inundacao-mas-previsao-de-chuva-mantem-atencao-em-porto-alegre.html
    /// - -1.18 m adjustment to mean sea level (ANA/SGB joint technical note): https://www.gov.br/ana/pt-br/assuntos/noticias-e-eventos/noticias/ana-e-servico-geologico-do-brasil-fazem-readequacoes-em-estacao-de-monitoramento-do-rio-guaiba-em-porto-alegre-rs
    /// - Physical gauge thresholds (3.15 m / 3.60 m), for contrast: https://www.estado.rs.gov.br/estado-atualiza-cota-de-inundacao-do-guaiba-na-usina-do-gasometro
    /// </remarks>
    /// 
     public static readonly MonitoringStation Guaiba = new(
        Code: "87450020",
        Name: "Usina do Gasômetro",
        RegionName: "Porto Alegre — Guaíba",
        Thresholds: new RiverThresholds(
            AttentionMeter: 2.00m,
            AlertMeter: 2.55m,     
            FloodMeter: 2.60m));   
}