using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Meta.Crawler.Models;

[Serializable]
internal sealed class TvChannel
{
    [JsonProperty("number")]
    [JsonPropertyName("number")]
    internal int Number { get; set; }

    [JsonProperty("name")]
    [JsonPropertyName("name")]
    internal string Name { get; set; }
}
