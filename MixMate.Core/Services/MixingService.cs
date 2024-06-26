using MixMate.Core.Entities;

namespace MixMate.Core.Services;

public class MixingService(IEnumerable<IMixingTechnique> mixingTechniques)
{
    private readonly IEnumerable<IMixingTechnique> _mixingTechniques = mixingTechniques;

    public List<Song> GetSuggestedSongs(string techniqueName, Song mainSong, List<Song> songs)
    {
        var suggestedSongs = new List<Song>();
        try
        {
            var technique = GetMixingTechniqueByName(techniqueName);
            suggestedSongs = technique.GetSuggestedSongs(mainSong, songs);
        }
        catch (ArgumentException ex) 
        {
            Console.WriteLine(ex.ToString()); //TODO: Logging
        }
        return suggestedSongs;
    }

    private IMixingTechnique GetMixingTechniqueByName(string techniqueName)
    {
        foreach (var technique in _mixingTechniques)
        {
            if (technique.GetType().Name.Equals(techniqueName, StringComparison.OrdinalIgnoreCase))
            {
                return technique;
            }
        }
        throw new ArgumentException($"Mixing technique '{techniqueName}' not found.");
    }
}
