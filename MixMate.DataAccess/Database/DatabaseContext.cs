﻿using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MixMate.Core.Interfaces;
using System.Data;

namespace MixMate.DataAccess.Database;

public class DatabaseContext(IConfiguration configuration, ILogger<DatabaseContext> logger) : IDatabaseContext
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<DatabaseContext> _logger = logger;

    public IDbConnection CreateConnection() => new SqliteConnection(_configuration.GetConnectionString("DefaultConnection"));

    public async Task Initialize()
    {
        try
        {
            // Create database tables if they don't exist
            using var connection = CreateConnection();
            await _initSongs();

            async Task _initSongs() // Move these to their own class if/when they get numerous
            {
                var sql = @"
                    CREATE TABLE IF NOT EXISTS 
                    Songs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Title TEXT NOT NULL,
                        Artist TEXT NOT NULL,
                        Album TEXT,
                        Genre TEXT,
                        Bpm REAL,
                        Duration INTEGER, -- Store TimeSpan as ticks
                        Key TEXT NOT NULL,
                        DateAdded TEXT NOT NULL,
                        UNIQUE (Title, Artist)
                    );";

                await connection.ExecuteAsync(sql);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
}
