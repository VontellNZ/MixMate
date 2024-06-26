using FluentAssertions;
using MixMate.Core.Entities;
using MixMate.Core.Enums;
using MixMate.Core.Services;

namespace MixMate.Tests;

public class MixingServiceTests
{
    [Fact]
    public void GivenSmoothMixing_GetSuggestedSongs_ShouldReturnOnlyMatchingSong()
    {
        //Arrange
        var mainSong = new Song(
            0,
            "Solar System",
            "Sub Focus",
            "Solar System / Siren",
            "Drum & Bass",
            174.0,
            new TimeSpan(0, 4, 48),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(4, "A")),
            DateTime.UtcNow);
        var harmonicMatch = new Song(
            1,
            "Devotion (feat. Cameron Hayes)",
            "Dimension",
            "Organ",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 10),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(4, "A")),
            DateTime.UtcNow);
        var nonMatch = new Song(
            2,
            "Afterglow",
            "Wilkinson",
            "Lazers Not Included",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 45),
            new Key("G", Scale.Major, Signature.None, new CamelotScale(9, "B")),
            DateTime.UtcNow);
        var songs = new List<Song> { mainSong, harmonicMatch, nonMatch };

        var techniqueName = "SmoothMixingTechnique";
        var mixingService = new MixingService([new SmoothMixingTechnique(), new EnergyBoostMixingTechnique()]);

        //Act
        var result = mixingService.GetSuggestedSongs(techniqueName, mainSong, songs);

        //Assert
        result.Should().Contain(harmonicMatch);
        result.Should().NotContain(mainSong);
        result.Should().NotContain(nonMatch);
    }

    [Theory]
    [InlineData("G", Scale.Minor, Signature.None, 6, "A")]
    [InlineData("F", Scale.Minor, Signature.Sharp, 11, "A")]
    public void GivenEnergyBoostMixing_GetSuggestedSongs_ShouldReturnOnlyMatchingSong(string note, Scale scale, Signature signature, int camelotNumber, string letter)
    {
        //Arrange
        var matchKey = new Key(note, scale, signature, new CamelotScale(camelotNumber, letter));

        var mainSong = new Song(
            0,
            "Solar System",
            "Sub Focus",
            "Solar System / Siren",
            "Drum & Bass",
            174.0,
            new TimeSpan(0, 4, 48),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(4, "A")),
            DateTime.UtcNow);
        var harmonicMatch = new Song(
            1,
            "Devotion (feat. Cameron Hayes)",
            "Dimension",
            "Organ",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 10),
            matchKey,
            DateTime.UtcNow);
        var nonMatch = new Song(
            2,
            "Afterglow",
            "Wilkinson",
            "Lazers Not Included",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 45),
            new Key("G", Scale.Major, Signature.None, new CamelotScale(9, "B")),
            DateTime.UtcNow);
        var songs = new List<Song> { mainSong, harmonicMatch, nonMatch };

        var techniqueName = "EnergyBoostMixingTechnique";
        var mixingService = new MixingService([new SmoothMixingTechnique(), new EnergyBoostMixingTechnique()]);

        //Act
        var result = mixingService.GetSuggestedSongs(techniqueName, mainSong, songs);

        //Assert
        result.Should().Contain(harmonicMatch);
        result.Should().NotContain(mainSong);
        result.Should().NotContain(nonMatch);
    }
}
