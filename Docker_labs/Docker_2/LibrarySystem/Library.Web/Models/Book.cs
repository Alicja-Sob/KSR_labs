using System.Text.Json.Serialization;

namespace Library.Web.Models;

public class Book
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("isbn")]
    public string ISBN { get; set; } = string.Empty;

    // System.Text.Json automatycznie parsuje standardowy format ISO z WebApi
    [JsonPropertyName("releaseDate")]
    public DateTime ReleaseDate { get; set; }
}