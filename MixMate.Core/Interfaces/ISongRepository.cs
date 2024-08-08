using MixMate.Core.Entities;

namespace MixMate.Core.Interfaces;

public interface ISongRepository
{
    Task AddSongsAsync(List<Song> songs);
    Task<IEnumerable<Song>> GetAllSongsAsync();
}