using Azure.Data.Tables;

public interface IUserService
{
    Task<bool> UserExistsAsync(string login);
    Task CreateUserAsync(string login, string password);
}

public class UserService : IUserService
{
    private readonly TableClient _tableClient;

    public UserService(TableServiceClient serviceClient)
    {
        _tableClient = serviceClient.GetTableClient("Users");
        _tableClient.CreateIfNotExists();
    }

    public async Task<bool> UserExistsAsync(string login)
    {
        try
        {
            var result = await _tableClient.GetEntityAsync<UserEntity>("USER", login);
            return result.Value != null;
        }
        catch
        {
            return false;
        }
    }

    public async Task CreateUserAsync(string login, string password)
    {
        var entity = new UserEntity
        {
            PartitionKey = "USER",
            RowKey = login,
            Password = password
        };

        await _tableClient.AddEntityAsync(entity);
    }
}