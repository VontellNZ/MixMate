using MixMate.Core.Entities;

namespace MixMate.Web.Interfaces;

public interface IMixMateClient
{
    Task<(bool Success, string Message, int SongsAdded)> AddSongsAsync(List<Song> songs);
    Task<List<Song>> GetAllSongsAsync();
}