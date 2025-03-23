using MixMate.Core.Entities;
using MixMate.Web.Interfaces;

namespace MixMate.Web.Services;

public class MixMateClient : IMixMateClient
{
    private readonly IMixMateGraphQLClient _client;
    private readonly ILogger<MixMateClient> _logger;

    public MixMateClient(
        IMixMateGraphQLClient getAllSongsQuery,
        ILogger<MixMateClient> logger)
    {
        _client = getAllSongsQuery;
        _logger = logger;
    }

    public async Task<List<Song>> GetAllSongsAsync()
    {
        try
        {
            var result = await _client.GetAllSongs.ExecuteAsync();

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

    public async Task<(bool Success, string Message, int SongsAdded)> AddSongsAsync(List<Song> songs)
    {
        try
        {
            // Convert your domain Songs to the input format required by GraphQL
            var songInputs = songs.Select(song => new SongInput
            {
                Title = song.Title,
                Artist = song.Artist,
                Album = song.Album,
                Genre = song.Genre,
                Bpm = song.Bpm,
                Duration = song.Duration,
                Key = new KeyInput
                {
                    Note = song.Key.Note,
                    Scale = song.Key.Scale.ToString(),
                    Signature = song.Key.Signature.ToString()
                }
                // Let the API set the DateAdded to current time if not provided
            }).ToList();

            var result = await _client.AddSongs.ExecuteAsync(songInputs);

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError("GraphQL error: {Message}", error.Message);
                }
                return (false, "Error adding songs", 0);
            }

            var payload = result.Data?.AddSongs;
            return (payload?.Success ?? false, payload?.Message ?? "Unknown error", payload?.SongsAdded ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding songs via GraphQL API");
            throw;
        }
    }
}