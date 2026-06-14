using System.Diagnostics;
using System.Text.Json;
using Library.Web.Configuration;
using Library.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Library.Web.Controllers;

public class HomeController : Controller
{
    private readonly EnvironmentConfig _environmentConfiguration;
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IOptions<EnvironmentConfig> configuration, IHttpClientFactory httpClientFactory)
    {
        _environmentConfiguration = configuration.Value;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        var endpointUrl = $"{_environmentConfiguration.LibraryWebApiServiceHost}/api/library/rented/";

        try
        {
            var response = await client.GetAsync(endpointUrl);
            response.EnsureSuccessStatusCode();

            // Nowoczesna asynchroniczna deserializacja bezpoúrednio ze strumienia danych
            var stream = await response.Content.ReadAsStreamAsync();
            var rentedBooks = await JsonSerializer.DeserializeAsync<List<LibraryResource>>(stream);

            var model = rentedBooks?.Select(resource => $"{resource?.Id} - {resource?.Book?.Title}, {resource?.Book?.Author}").ToList() ?? [];

            return View(model);
        }
        catch (Exception ex)
        {
            // Zabezpieczenie na wypadek, gdy student nie uruchomi≥ jeszcze serwisu WebApi w Dockerze
            Console.WriteLine($"B≥πd podczas odpytywania API: {ex.Message}");
            return View(new List<string> { "Brak po≥πczenia z Library.WebApi. Sprawdü, czy kontener na porcie 91 dzia≥a." });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}