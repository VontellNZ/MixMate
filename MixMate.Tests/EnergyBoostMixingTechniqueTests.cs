using FluentAssertions;
using MixMate.Core.Entities;
using MixMate.Core.Enums;
using MixMate.Core.Services;

namespace MixMate.Tests;

public class EnergyBoostMixingTechniqueTests
{
    #region GetSuggestedSongs

    [Fact]
    public void GivenListOfSongs_GetSuggestedSongs_ShouldReturnMatchingSongs()
    {
        //Arrange
        var mainSong = new Song(
            0,
            "Solar System",
            "Sub Focus",
            "Solar System / Siren",
            "Drum & Bass",
            174,
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
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(6, "A")),
            DateTime.UtcNow);
        var nonMatch = new Song(
            2,
            "Afterglow",
            "Wilkinson",
            " Lazers Not Included",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 45),
            new Key("G", Scale.Major, Signature.None, new CamelotScale(6, "B")),
            DateTime.UtcNow);
        var songs = new List<Song> { mainSong, harmonicMatch, nonMatch };

        var mixingTechnique = new EnergyBoostMixingTechnique();

        //Act
        var result = mixingTechnique.GetSuggestedSongs(mainSong, songs);

        //Assert
        result.Should().Contain(harmonicMatch);
        result.Should().NotContain(mainSong);
        result.Should().NotContain(nonMatch);
    }

    [Theory]
    [InlineData(11, 1)]
    [InlineData(12, 2)]
    public void GivenWraparoundKey_GetSuggestedSongs_ShouldReturnMatchingSong(int mainCamelotNumber, int matchingCamelotNumber)
    {
        //Arrange
        var mainSong = new Song(
            0,
            "Solar System",
            "Sub Focus",
            "Solar System / Siren",
            "Drum & Bass",
            174,
            new TimeSpan(0, 4, 48),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(mainCamelotNumber, "A")),
            DateTime.UtcNow);
        var harmonicMatch = new Song(
            1,
            "Devotion (feat. Cameron Hayes)",
            "Dimension",
            "Organ",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 10),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(matchingCamelotNumber, "A")),
            DateTime.UtcNow);
        var nonMatch = new Song(
            2,
            "Afterglow",
            "Wilkinson",
            " Lazers Not Included",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 45),
            new Key("G", Scale.Major, Signature.None, new CamelotScale(matchingCamelotNumber, "B")),
            DateTime.UtcNow);
        var songs = new List<Song> { mainSong, harmonicMatch, nonMatch };

        var mixingTechnique = new EnergyBoostMixingTechnique();

        //Act
        var result = mixingTechnique.GetSuggestedSongs(mainSong, songs);

        //Assert
        result.Should().Contain(harmonicMatch);
        result.Should().NotContain(mainSong);
        result.Should().NotContain(nonMatch);
    }

    [Fact]
    public void GivenAvbVariation_GetSuggestedSongs_ShouldReturnMatchingSongs()
    {
        //Arrange
        var mainSong = new Song(
            0,
            "Solar System",
            "Sub Focus",
            "Solar System / Siren",
            "Drum & Bass",
            174,
            new TimeSpan(0, 4, 48),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(6, "A")),
            DateTime.UtcNow);
        var harmonicMatch = new Song(
            1,
            "Devotion (feat. Cameron Hayes)",
            "Dimension",
            "Organ",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 10),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(1, "A")),
            DateTime.UtcNow);
        var nonMatch = new Song(
            2,
            "Afterglow",
            "Wilkinson",
            " Lazers Not Included",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 45),
            new Key("G", Scale.Major, Signature.None, new CamelotScale(1, "B")),
            DateTime.UtcNow);
        var songs = new List<Song> { mainSong, harmonicMatch, nonMatch };

        var mixingTechnique = new EnergyBoostMixingTechnique();

        //Act
        var result = mixingTechnique.GetSuggestedSongs(mainSong, songs);

        //Assert
        result.Should().Contain(harmonicMatch);
        result.Should().NotContain(mainSong);
        result.Should().NotContain(nonMatch);
    }

    [Theory]
    [InlineData(5, 12)]
    [InlineData(1, 8)]
    public void GivenWraparoundAvbVariation_GetSuggestedSongs_ShouldReturnMatchingSong(int mainCamelotNumber, int matchingCamelotNumber)
    {
        //Arrange
        var mainSong = new Song(
            0,
            "Solar System",
            "Sub Focus",
            "Solar System / Siren",
            "Drum & Bass",
            174,
            new TimeSpan(0, 4, 48),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(mainCamelotNumber, "A")),
            DateTime.UtcNow);
        var harmonicMatch = new Song(
            1,
            "Devotion (feat. Cameron Hayes)",
            "Dimension",
            "Organ",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 10),
            new Key("F", Scale.Minor, Signature.None, new CamelotScale(matchingCamelotNumber, "A")),
            DateTime.UtcNow);
        var nonMatch = new Song(
            2,
            "Afterglow",
            "Wilkinson",
            " Lazers Not Included",
            "Drum & Bass",
            174,
            new TimeSpan(0, 3, 45),
            new Key("G", Scale.Major, Signature.None, new CamelotScale(matchingCamelotNumber, "B")),
            DateTime.UtcNow);
        var songs = new List<Song> { mainSong, harmonicMatch, nonMatch };

        var mixingTechnique = new EnergyBoostMixingTechnique();

        //Act
        var result = mixingTechnique.GetSuggestedSongs(mainSong, songs);

        //Assert
        result.Should().Contain(harmonicMatch);
        result.Should().NotContain(mainSong);
        result.Should().NotContain(nonMatch);
    }

    #endregion GetSuggestedSongs

    #region GetModifiedCamelotScale

    [Theory]
    [InlineData(6, "A", 2, 8)]
    [InlineData(6, "B", 2, 8)]
    [InlineData(6, "A", -5, 1)]
    [InlineData(6, "B", -5, 1)]
    public void GivenScalesWithinBounds_GetModifiedCamelotScale_ShouldCorrectlyModifyScale(int mainSongNumber, string mainSongLetter, int energyBoostModifier, int expectedNumber)
    {
        //Arrange
        var mixingTechnique = new EnergyBoostMixingTechnique();
        var mainSongCamelotScale = new CamelotScale(mainSongNumber, mainSongLetter);

        //Act
        var result = mixingTechnique.GetModifiedCamelotScale(mainSongCamelotScale, energyBoostModifier);

        //Assert
        result.Number.Should().Be(expectedNumber);
        result.Letter.Should().Be(mainSongCamelotScale.Letter);
    }

    [Theory]
    [InlineData(12, 2, 2)]
    [InlineData(11, 2, 1)]
    [InlineData(5, -5, 12)]
    [InlineData(4, -5, 11)]
    [InlineData(3, -5, 10)]
    [InlineData(2, -5, 9)]
    [InlineData(1, -5, 8)]
    public void GivenScalesOutsideBounds_GetModifiedCamelotScale_ShouldCorrectlyModifyScale(int mainSongNumber, int energyBoostModifier, int expectedNumber)
    {
        //Arrange
        var mixingTechnique = new EnergyBoostMixingTechnique();
        var mainSongCamelotScale = new CamelotScale(mainSongNumber, "A");

        //Act
        var result = mixingTechnique.GetModifiedCamelotScale(mainSongCamelotScale, energyBoostModifier);

        //Assert
        result.Number.Should().Be(expectedNumber);
        result.Letter.Should().Be(mainSongCamelotScale.Letter);
    }

    [Fact]
    public void GivenCamelotScaleLetter_GetModifiedCamelotScale_ShouldNotModifyLetter()
    {
        //Arrange
        var mixingTechnique = new EnergyBoostMixingTechnique();
        var mainSongCamelotScale = new CamelotScale(2, "A");

        //Act
        var result = mixingTechnique.GetModifiedCamelotScale(mainSongCamelotScale, 2);

        //Assert
        result.Letter.Should().Be(mainSongCamelotScale.Letter);
    }

    #endregion GetModifiedCamelotScale

}
