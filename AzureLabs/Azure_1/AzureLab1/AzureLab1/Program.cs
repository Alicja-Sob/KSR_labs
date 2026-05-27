using Azure.Core;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using AzureTableAuthApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DI container, dependency injection
builder.Services.AddAzureClients(clientBuilder =>
{
    string connString = builder.Configuration.GetConnectionString("AzureStorage");

    clientBuilder.AddTableServiceClient(connString);
    clientBuilder.AddBlobServiceClient(connString);
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();