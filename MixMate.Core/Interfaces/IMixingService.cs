using MixMate.Core.Entities;

namespace MixMate.Core.Interfaces
{
    public interface IMixingService
    {
        List<string> GetAvailableMixingTechniqueNames();
        List<Song> GetSuggestedSongs(string techniqueName, Song mainSong, List<Song> songs);
    }
}