using Dapper;
using MixMate.Core.Entities;
using MixMate.DataAccess.Database;
using MixMate.DataAccess.Exceptions;

namespace MixMate.DataAccess.Repositories;

public class SongRepository(IDatabaseContext dbContext)
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
        var sql = "INSERT INTO Songs (Title, Artist, Album, Genre, Bpm, Duration, Note, Scale, Signature, CamelotScaleNumber, CamelotScaleLetter, DateAdded) " +
              "VALUES (@Title, @Artist, @Album, @Genre, @Bpm, @Duration, @Note, @Scale, @Signature, @CamelotScaleNumber, @CamelotScaleLetter, @DateAdded)";

        foreach (var song in songs)
        {
            try
            {
                await connection.ExecuteAsync(sql, new
                {
                    song.Title,
                    song.Artist,
                    song.Album,
                    song.Genre,
                    song.Bpm,
                    Duration = song.Duration.Ticks,
                    song.Key.Note,
                    song.Key.Scale,
                    song.Key.Signature,
                    CamelotScaleNumber = song.Key.CamelotScale.Number,
                    CamelotScaleLetter = song.Key.CamelotScale.Letter,
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
}