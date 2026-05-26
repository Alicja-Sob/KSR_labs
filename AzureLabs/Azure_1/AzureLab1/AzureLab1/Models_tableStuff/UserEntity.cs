using Azure;
using Azure.Data.Tables;

public class UserEntity : ITableEntity
{
    public UserEntity(string rk, string pk = "USER")
    {
        this.PartitionKey = pk;
        this.RowKey = rk; //klucz glowny | login
    }
    public UserEntity() { }

    public string PartitionKey { get; set; } = "USER";
    public required string RowKey { get; set; }
    public string Password { get; set; } = default!;

    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

}