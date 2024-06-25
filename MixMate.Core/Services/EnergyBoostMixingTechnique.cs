using MixMate.Core.Entities;

namespace MixMate.Core.Services;

public class EnergyBoostMixingTechnique : IMixingTechnique
{
    private readonly List<int> EnergyBoostModifiers = [2, -5];
    private const int _maximumCamelotNumber = 12;

    public List<Song> GetSuggestedSongs(Song mainSong, List<Song> songs)
    {
        songs.Remove(mainSong);
        var suggestedSongs = new List<Song>();

        foreach (var energyBoostModifier in EnergyBoostModifiers)
        {
            var modifiedCamelotScale = GetModifiedCamelotScale(mainSong.Key.CamelotScale, energyBoostModifier);
            var matchingKeySongs = songs.Where(s => s.Key.CamelotScale.Equals(modifiedCamelotScale));
            suggestedSongs.AddRange(matchingKeySongs);
        }

        return suggestedSongs;
    }

    private static CamelotScale GetModifiedCamelotScale(CamelotScale mainSongCamelotScale, int energyBoostModifier)
    {
        var modifiedCamelotScale = new CamelotScale(mainSongCamelotScale.Number + energyBoostModifier, mainSongCamelotScale.Letter);

        //Number exceeeded the maximum, wrap around from 1
        if (modifiedCamelotScale.Number > _maximumCamelotNumber)
        {
            modifiedCamelotScale.Number = modifiedCamelotScale.Number - _maximumCamelotNumber;
        }
        //Number is zero or less, wrap back around from the maximum
        else if (modifiedCamelotScale.Number <= 0)
        {
            modifiedCamelotScale.Number = _maximumCamelotNumber + modifiedCamelotScale.Number;
        }

        return modifiedCamelotScale;
    }
}
