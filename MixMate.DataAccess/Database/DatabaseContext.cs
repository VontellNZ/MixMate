using Microsoft.Data.Sqlite;
using MixMate.Core.Interfaces;
using System.Data;

namespace MixMate.DataAccess.Database;

public class DatabaseContext(string connectionString) : IDatabaseContext
{
    private readonly string _connectionString = connectionString;

    public IDbConnection CreateConnection() => new SqliteConnection(_connectionString);
}
