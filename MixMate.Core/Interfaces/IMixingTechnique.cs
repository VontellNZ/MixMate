using MixMate.Core.Entities;

namespace MixMate.Core.Interfaces;

public interface IMixingTechnique
{
    List<Song> GetSuggestedSongs(Song mainSong, List<Song> songs);
}