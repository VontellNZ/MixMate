using MixMate.Core.Entities;

namespace MixMate.Web.Interfaces;

public interface IMixMateClient
{
    Task<List<Song>> GetAllSongsAsync();
}