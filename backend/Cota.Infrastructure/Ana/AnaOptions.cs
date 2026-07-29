namespace Cota.Infrastructure.Ana;

public class AnaOptions
{
    public const string SectionName = "Ana";
    public string Identificador { get; set; } = "";
    public string Senha { get; set; } = "";
    public string BaseUrl { get; set; } = "https://www.ana.gov.br/hidrowebservice/";
}