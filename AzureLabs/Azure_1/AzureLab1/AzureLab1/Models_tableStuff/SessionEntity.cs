using Azure;
using Azure.Data.Tables;

public class SessionEntity : ITableEntity
{
    public SessionEntity(string rk, string pk = "SESSION")
    {
        this.PartitionKey = pk;
        this.RowKey = rk; //login dla ktorego tworzymy sesje
    }
    public SessionEntity() { }

    public string PartitionKey { get; set; } = "SESSION";
    public required string RowKey { get; set; } = default!;
    public string Password { get; set; } = default!;
    public Guid SessionId { get; set; } = default!;


    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

}
