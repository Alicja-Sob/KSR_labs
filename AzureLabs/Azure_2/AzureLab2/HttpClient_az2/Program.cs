using System.Net.Http.Json;

Console.WriteLine("Starting a new http testing clinet");

using HttpClient client = new HttpClient();
client.BaseAddress = new Uri("https://localhost:7290/");    

var files = new List<(string name, string content)>
{
    ("testFileName1", "testContent_abc_xyz_123"),
    ("testFileName2", "testContent_abc_xyz_1234"),
    ("testFileName3", "testContent_abc_xyz_321"),
    ("testFileName4", "testContent_abc_xyz_467"),
    ("testFileName5", "testContent_abc_xyz_356"),
};

Console.WriteLine("----------------------------------------------");
Console.WriteLine("TEST 1 - SIMULATANIOUS SENDING of 5 FILES");
Console.WriteLine("----------------------------------------------");

// Wysyła w pętli dokładnie 5 różnych żądań do punktu końcowego /api/processing/encode (symulując jednoczesne przesłanie 5 plików do przetwarzania).
foreach (var file in files)
{
    var response = await client.PostAsJsonAsync(
        "/api/processing/encode",
        new
        {
            fileName = file.name,
            fileContent = file.content
        });

    Console.WriteLine($"Sent: {file.name} -> {response.StatusCode}");
}

//Wprowadza wymuszone opóźnienie trwające dokładnie 4 sekundy (Task.Delay(TimeSpan.FromSeconds(4))).

Console.WriteLine("----------------------------------------------");
Console.WriteLine("TEST 2 - WAITING 4 SECONDS");
Console.WriteLine("----------------------------------------------");

await Task.Delay(TimeSpan.FromSeconds(4));

// Po upływie tego czasu, odpytuje po kolei punkt końcowy /api/processing/download/{nazwa} dla każdego z 5 plików i wypisuje wyniki w konsoli.

Console.WriteLine("----------------------------------------------");
Console.WriteLine("TEST 3 - DOWNLOADING RESULTS");
Console.WriteLine("----------------------------------------------");

foreach (var file in files)
{
    var result =
        await client.GetStringAsync($"/api/processing/download/{file.name}");

    Console.WriteLine($"{file.name} : {result}");

}


Console.WriteLine("----------------------------------------------");
Console.WriteLine("TESTING DONE");
Console.WriteLine("----------------------------------------------");