using Microsoft.Extensions.Logging;
using MixMate.Core.Entities;
using MixMate.Core.Interfaces;

namespace MixMate.Core.Services;

public class MixingService(IEnumerable<IMixingTechnique> mixingTechniques, ILogger<MixingService> logger) : IMixingService
{
    public List<string> AvailableMixingTechniqueNames => _mixingTechniques.Select(technique => technique.GetType().Name).ToList();
    private readonly IEnumerable<IMixingTechnique> _mixingTechniques = mixingTechniques;
    private readonly ILogger<MixingService> _logger = logger;

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
            _logger.LogError(
                ex,
                "Error while getting suggested songs for {Song} using {MixingTechnique} mixing technique",
                mainSong.Title,
                techniqueName);
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