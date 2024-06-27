using System.Text.Json.Serialization;

namespace Revenue_Recognition_System.Models;

public class ExchangeModel
{
    [JsonPropertyName("base_code")]
    public string Base { get; set; }
    [JsonPropertyName("conversion_rates")]
    public Dictionary<string, decimal> Rates { get; set; }
}