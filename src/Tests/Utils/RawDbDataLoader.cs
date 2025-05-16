using MySql.Data.MySqlClient;
using System.Data;

public class RawDbDataLoader : IDisposable
{
    private readonly MySqlConnection _connection;
    private bool _disposed;

    public RawDbDataLoader(string sqlConnectionString)
    {
        if (string.IsNullOrWhiteSpace(sqlConnectionString))
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(sqlConnectionString));

        _connection = new MySqlConnection(sqlConnectionString);
    }

    public Task<List<Dictionary<string, object?>>> AllUsersAsync()
        => LoadFromMysqlAsync("SELECT * FROM User ORDER BY Id");

    public Task<List<Dictionary<string, object?>>> AllQuestionsAsync()
        => LoadFromMysqlAsync("SELECT * FROM Question ORDER BY Id");

    public Task<List<Dictionary<string, object?>>> AllPagesAsync()
        => LoadFromMysqlAsync("SELECT * FROM Page ORDER BY Id");

    private async Task<List<Dictionary<string, object?>>> LoadFromMysqlAsync(string query)
    {
        var results = new List<Dictionary<string, object?>>();

        await using var connection = new MySqlConnection(_connection.ConnectionString);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object?>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var value = reader.GetValue(i);
                row[columnName] = value == DBNull.Value ? null : value;
            }

            results.Add(row);
        }

        return results;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();

            _connection.Dispose();
        }

        _disposed = true;
    }
}