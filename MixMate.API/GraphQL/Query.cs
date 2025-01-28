using MixMate.Core.Entities;
using MixMate.Core.Interfaces;

namespace MixMate.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<Song>> GetSongs([Service] ISongService songService) => await songService.GetAllSongsAsync();
}
