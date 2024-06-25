using MixMate.Core.Entities;

namespace MixMate.Core.Services;

public class MixingService(IEnumerable<IMixingTechnique> mixingTechniques)
{
    private readonly IEnumerable<IMixingTechnique> _mixingTechniques = mixingTechniques;

    public List<Song> GetSuggestedSongs(string techniqueName, Song mainSong, List<Song> songs)
    {
        var technique = GetMixingTechniqueByName(techniqueName) 
            ?? throw new ArgumentException($"Mixing technique '{techniqueName}' not found.");

        return technique.GetSuggestedSongs(mainSong, songs);
    }

    private IMixingTechnique? GetMixingTechniqueByName(string name)
    {
        foreach (var technique in _mixingTechniques)
        {
            if (technique.GetType().Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return technique;
            }
        }
        return null;
    }
}
