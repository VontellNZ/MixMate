using Dapper;
using MixMate.Core.Entities;
using MixMate.Core.Interfaces;
using MixMate.DataAccess.Exceptions;
using System.Data;

namespace MixMate.DataAccess.Repositories;

public class SongRepository(IDatabaseContext dbContext) : ISongRepository
{
    private readonly IDatabaseContext _dbContext = dbContext;

    public async Task<IEnumerable<Song>> GetAllSongsAsync()
    {
        try
        {
            using var connection = _dbContext.CreateConnection();
            return await connection.QueryAsync<Song>("SELECT * FROM Songs");
        }
        catch (Exception ex)
        {
            throw new SongRepositoryException("An error occurred while retrieving all songs from the database", ex);
        }
    }

    public async Task AddSongsAsync(List<Song> songs)
    {
        using var connection = _dbContext.CreateConnection();
        var sql = "INSERT INTO Songs (Title, Artist, Album, Genre, Bpm, Duration, Key, DateAdded) " +
              "VALUES (@Title, @Artist, @Album, @Genre, @Bpm, @Duration, @Key, @DateAdded)";

        foreach (var song in songs)
        {
            try
            {
                if (await SongExistsInDatabase(connection, song)) continue;

                await connection.ExecuteAsync(sql, new
                {
                    song.Title,
                    song.Artist,
                    song.Album,
                    song.Genre,
                    song.Bpm,
                    Duration = song.Duration.Ticks,
                    Key = song.Key.GetFullKey(),
                    song.DateAdded
                });
            }
            catch (Exception ex)
            {
                //logging
                throw new SongRepositoryException($"An error occurred while adding the song with title {song.Title}.", ex);
            }
        }
    }

    private static async Task<bool> SongExistsInDatabase(IDbConnection connection, Song song)
    {
        var songExists = await connection.QueryAsync<Song>(
                    "SELECT 1 WHERE EXISTS (SELECT 1 FROM Songs WHERE Title = @Title AND Artist = @Artist)", new { song.Title, song.Artist });

        return songExists != null && songExists.Any();
    }
}