using Library.Web.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja nadpisywania konfiguracji zmiennymi œrodowiskowymi (jak w starym kodzie)
builder.Configuration.AddEnvironmentVariables();

// Wstrzykiwanie konfiguracji z appsettings.json
builder.Services.Configure<EnvironmentConfig>(builder.Configuration);

// Rejestracja kontrolerów, widoków i fabryki HttpClient
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

var app = builder.Build();

// Konfiguracja potoku ¿¹dañ HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();