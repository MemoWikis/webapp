using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Generic summary object that returns the number of elements,
/// the first element, and the last element of a sequence.
/// </summary>
public class Summary<TElement>
{
    public int Count { get; }
    public TElement? First { get; }
    public TElement? Last { get; }

    public Summary(int count, TElement? first, TElement? last)
    {
        Count = count;
        First = first;
        Last = last;
    }
}

public class RawDbDataLoader : IDisposable
{
    private readonly MySqlConnection _connection;
    private bool _disposed;

    public RawDbDataLoader(string sqlConnectionString)
    {
        if (string.IsNullOrWhiteSpace(sqlConnectionString))
        {
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(sqlConnectionString));
        }

        _connection = new MySqlConnection(sqlConnectionString);
    }

    public Task<List<Dictionary<string, object?>>> AllUsersAsync()
        => LoadFromMysqlAsync("SELECT * FROM User ORDER BY Id");

    public async Task<Summary<Dictionary<string, object?>>> AllUsersSummaryAsync()
        => Summary(await AllUsersAsync());

    public Task<List<Dictionary<string, object?>>> AllQuestionsAsync()
        => LoadFromMysqlAsync("SELECT * FROM Question ORDER BY Id");

    public async Task<Summary<Dictionary<string, object?>>> AllQuestionsSummaryAsync()
        => Summary(await AllQuestionsAsync());

    public Task<List<Dictionary<string, object?>>> AllPagesAsync()
        => LoadFromMysqlAsync("SELECT * FROM Page ORDER BY Id");

    public async Task<Summary<Dictionary<string, object?>>> AllPagesSummaryAsync()
        => Summary(await AllPagesAsync());

    private static Summary<Dictionary<string, object?>> Summary(List<Dictionary<string, object?>> items)
    {
        if (items.Count == 0)
            return new Summary<Dictionary<string, object?>>(0, null, null);

        Dictionary<string, object?> firstRow = items.First();
        Dictionary<string, object?> lastRow = items.Last();

        return new Summary<Dictionary<string, object?>>(items.Count, firstRow, lastRow);
    }

    private async Task<List<Dictionary<string, object?>>> LoadFromMysqlAsync(string query)
    {
        List<Dictionary<string, object?>> results = new List<Dictionary<string, object?>>();

        await using MySqlConnection connection = new MySqlConnection(_connection.ConnectionString);
        await connection.OpenAsync().ConfigureAwait(false);

        await using MySqlCommand command = new MySqlCommand(query, connection);
        await using DbDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(false);

        while (await reader.ReadAsync().ConfigureAwait(false))
        {
            Dictionary<string, object?> row = new Dictionary<string, object?>();
            for (int columnIndex = 0; columnIndex < reader.FieldCount; columnIndex++)
            {
                string columnName = reader.GetName(columnIndex);
                object? value = reader.GetValue(columnIndex);
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
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection.Dispose();
        }

        _disposed = true;
    }
}
