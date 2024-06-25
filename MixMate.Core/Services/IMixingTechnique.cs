using MixMate.Core.Entities;

namespace MixMate.Core.Services;

public interface IMixingTechnique
{
    List<Song> GetSuggestedSongs(Song mainSong, List<Song> songs);
}