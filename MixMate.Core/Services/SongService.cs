using MixMate.Core.Entities;
using MixMate.Core.Interfaces;

namespace MixMate.Core.Services;

public class SongService(ISongRepository songRepository) : ISongService
{
    private readonly ISongRepository _songRepository = songRepository;

    public async Task<IEnumerable<Song>> GetAllSongsAsync() => await _songRepository.GetAllSongsAsync();

    public async Task AddSongsAsync(List<Song> songs) => await _songRepository.AddSongsAsync(songs);
}
