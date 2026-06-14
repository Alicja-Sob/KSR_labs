using System.Text.Json.Serialization;

namespace Library.Web.Models;

public class LibraryResource
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("book")]
    public Book? Book { get; set; }
}