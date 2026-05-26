using Azure;
using Azure.Data.Tables;

namespace AzureTableAuthApi.Services;

public interface IAuthorizationService
{
    Task<Guid?> LoginAsync(string login, string password);
    Task<bool> LogoutAsync(string login);
}

public class AuthorizationService : IAuthorizationService
{
    private readonly TableClient _usersTable;
    private readonly TableClient _sessionsTable;

    public AuthorizationService(TableServiceClient serviceClient)
    {
        _usersTable = serviceClient.GetTableClient("Users");
        _usersTable.CreateIfNotExists();

        _sessionsTable = serviceClient.GetTableClient("Sessions");
        _sessionsTable.CreateIfNotExists();
    }

    public async Task<Guid?> LoginAsync(string login, string password)
    {
        try
        {
            var userResponse = await _usersTable.GetEntityAsync<UserEntity>("USER", login);
            var user = userResponse.Value;

            if (user == null)
                return null;

            if (user.Password != password)
                return null;

            var sessionId = Guid.NewGuid();

            var session = new SessionEntity
            {
                PartitionKey = "SESSION",
                RowKey = login,
                SessionId = sessionId
            };

            await _sessionsTable.UpsertEntityAsync(session);

            return sessionId;
        }
        catch (RequestFailedException)
        {
            return null;
        }
    }
    public async Task<bool> LogoutAsync(string login)
    {
        try
        {
            var response = await _sessionsTable.GetEntityIfExistsAsync<SessionEntity>(
                "SESSION",
                login
            );

            if (!response.HasValue)
                return false;

            await _sessionsTable.DeleteEntityAsync("SESSION", login);

            return true;
        }
        catch (RequestFailedException)
        {
            return false;
        }
    }
}