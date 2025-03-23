using MixMate.Core.Entities;
using MixMate.Core.Enums;
using MixMate.Core.Interfaces;

namespace MixMate.API.GraphQL
{
    public class Mutation
    {
        public async Task<AddSongsPayload> AddSongs([Service] ISongRepository songRepository, IEnumerable<SongInput> songs)
        {
            try
            {
                var songsToAdd = songs.Select(newSong => new Song(
                    0, // ID will be assigned by the database
                    newSong.Title,
                    newSong.Artist,
                    newSong.Album,
                    newSong.Genre,
                    newSong.Bpm,
                    newSong.Duration,
                    new Key(
                        newSong.Key.Note,
                        Enum.Parse<Scale>(newSong.Key.Scale.ToString(), true),
                        Enum.Parse<Signature>(newSong.Key.Signature.ToString(), true)
                    ),
                    newSong.DateAdded ?? DateTime.UtcNow
                )).ToList();

                await songRepository.AddSongsAsync(songsToAdd);

                return new AddSongsPayload
                {
                    Success = true,
                    Message = $"Successfully added {songsToAdd.Count} songs",
                    SongsAdded = songsToAdd.Count
                };
            }
            catch (Exception ex)
            {
                return new AddSongsPayload
                {
                    Success = false,
                    Message = $"Error adding songs: {ex.Message}",
                    SongsAdded = 0
                };
            }
        }
    }
}