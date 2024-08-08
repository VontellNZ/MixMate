using MixMate.Core.Entities;
using MixMate.Core.Interfaces;

namespace MixMate.Core.Services;

public class SmoothMixingTechnique : IMixingTechnique
{
    public List<Song> GetSuggestedSongs(Song mainSong, List<Song> songs)
    {
        songs.Remove(mainSong);

        var suggestedSongs = new List<Song>();
        var keyRange = Enumerable.Range(mainSong.Key.CamelotScale.Number - 1, mainSong.Key.CamelotScale.Number + 1);
        foreach (var song in songs)
        {
            if (keyRange.Contains(song.Key.CamelotScale.Number))
                suggestedSongs.Add(song);
            else if (mainSong.Key.CamelotScale.Number.Equals(1) && song.Key.CamelotScale.Number.Equals(12))
                suggestedSongs.Add(song);
            else if (mainSong.Key.CamelotScale.Number.Equals(12) && song.Key.CamelotScale.Number.Equals(1))
                suggestedSongs.Add(song);
        }
        return suggestedSongs;
    }
}
