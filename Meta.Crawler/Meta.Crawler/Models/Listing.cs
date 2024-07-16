using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Meta.Crawler.Models;

[Serializable]
internal sealed class Listing
{
    [JsonProperty("timestamp")]
    [JsonPropertyName("timestamp")]
    internal string Timestamp { get; set; }

    [JsonProperty("title")]
    [JsonPropertyName("title")]
    internal string Title { get; set; }
}
