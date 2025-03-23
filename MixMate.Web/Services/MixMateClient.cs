using MixMate.Core.Entities;
using MixMate.Web.Interfaces;

namespace MixMate.Web.Services;

public class MixMateClient : IMixMateClient
{
    private readonly GetAllSongsQuery _getAllSongsQuery;
    private readonly ILogger<MixMateClient> _logger;

    public MixMateClient(
        GetAllSongsQuery getAllSongsQuery,
        ILogger<MixMateClient> logger)
    {
        _getAllSongsQuery = getAllSongsQuery;
        _logger = logger;
    }

    public async Task<List<Song>> GetAllSongsAsync()
    {
        try
        {
            var result = await _getAllSongsQuery.ExecuteAsync();

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("GraphQL error: {Message}", error.Message);
                }
                return [];
            }

            var songs = new List<Song>();

            foreach (var graphqlSong in result.Data?.AllSongs ?? [])
            {
                var key = new Key(
                    graphqlSong.Key.Note,
                    Enum.Parse<Core.Enums.Scale>(graphqlSong.Key.Scale.ToString(), true),
                    Enum.Parse<Core.Enums.Signature>(graphqlSong.Key.Signature.ToString(), true),
                    new CamelotScale(
                        graphqlSong.Key.CamelotScale.Number,
                        graphqlSong.Key.CamelotScale.Letter
                    )
                );

                var song = new Song(
                    graphqlSong.Id,
                    graphqlSong.Title,
                    graphqlSong.Artist,
                    graphqlSong.Album,
                    graphqlSong.Genre,
                    graphqlSong.Bpm,
                    graphqlSong.Duration,
                    key,
                    graphqlSong.DateAdded.DateTime
                );

                songs.Add(song);
            }

            return songs;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching songs from GraphQL API");
            throw;
        }
    }
}