using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAzureClients(clientBuilder =>
{
    string connString = builder.Configuration.GetConnectionString("AzureStorage");

    clientBuilder.AddQueueServiceClient(connString);
    clientBuilder.AddBlobServiceClient(connString);
});

builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IEncodedMsgRetrivalService, EncodedMsgRetrivalService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
