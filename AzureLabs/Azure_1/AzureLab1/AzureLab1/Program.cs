using Azure.Data.Tables;
using Azure.Core;
using AzureTableAuthApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Azure Table ServiceClient (Azurite)
builder.Services.AddSingleton(_ =>
{
    var connectionString = builder.Configuration["AzureStorage"];
    return new TableServiceClient(connectionString);
});

//SERVICES ---------------------------
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();