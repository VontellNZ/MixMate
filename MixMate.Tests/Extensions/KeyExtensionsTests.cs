using FluentAssertions;
using MixMate.Core.Entities;
using MixMate.Core.Enums;
using MixMate.Core.Extensions;

namespace MixMate.Core.Tests.Extensions;

public class KeyExtensionsTests
{
    [Theory]
    [InlineData("A", Signature.Flat, 1)]
    [InlineData("E", Signature.Flat, 2)]
    [InlineData("B", Signature.Flat, 3)]
    [InlineData("F", Signature.None, 4)]
    [InlineData("C", Signature.None, 5)]
    [InlineData("G", Signature.None, 6)]
    [InlineData("D", Signature.None, 7)]
    [InlineData("A", Signature.None, 8)]
    [InlineData("E", Signature.None, 9)]
    [InlineData("B", Signature.None, 10)]
    [InlineData("F", Signature.Sharp, 11)]
    [InlineData("D", Signature.Flat, 12)]
    public void GetCamelotScaleFromKey_ForMinorScale_ShouldReturnCorrectCamelotScale(string note, Signature signature, int expectedCamelotScaleNumber)
    {
        // Arrange
        var key = new Key(note, Scale.Minor, signature, new CamelotScale(0, "A")); // CamelotScale is set but will be replaced

        // Act
        var result = key.GetCamelotScaleFromKey();

        // Assert
        result.Number.Should().Be(expectedCamelotScaleNumber);
        result.Letter.Should().Be("A");
    }

    [Theory]
    [InlineData("B", Signature.None, 1)]
    [InlineData("F", Signature.Sharp, 2)]
    [InlineData("D", Signature.Flat, 3)]
    [InlineData("A", Signature.Flat, 4)]
    [InlineData("E", Signature.Flat, 5)]
    [InlineData("B", Signature.Flat, 6)]
    [InlineData("F", Signature.None, 7)]
    [InlineData("C", Signature.None, 8)]
    [InlineData("G", Signature.None, 9)]
    [InlineData("D", Signature.None, 10)]
    [InlineData("A", Signature.None, 11)]
    [InlineData("E", Signature.None, 12)]
    public void GetCamelotScaleFromKey_ForMajorScale_ShouldReturnCorrectCamelotScale(string note, Signature signature, int expectedCamelotScaleNumber)
    {
        // Arrange
        var key = new Key(note, Scale.Major, signature, new CamelotScale(0, "B")); // CamelotScale is set but will be replaced

        // Act
        var result = key.GetCamelotScaleFromKey();

        // Assert
        result.Number.Should().Be(expectedCamelotScaleNumber);
        result.Letter.Should().Be("B");
    }

    [Fact]
    public void GetCamelotScaleFromKey_ShouldThrowArgumentNullException_WhenKeyIsNull()
    {
        // Arrange
        Key? key = null;

        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        var act = () => key.GetCamelotScaleFromKey();
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    #region GetKeyFromString

    [Theory]
    [InlineData(null, typeof(ArgumentNullException))]
    [InlineData("H#", typeof(FormatException))]
    [InlineData("A#", typeof(KeyNotFoundException))]
    [InlineData("Am", null)]
    [InlineData("A#m", typeof(KeyNotFoundException))]
    [InlineData("A##", typeof(FormatException))]
    public void GetKeyFromString_ShouldThrowExceptions_ForInvalidStrings(string? value, Type? expectedExceptionType)
    {
        // Act
#pragma warning disable CS8604 // Possible null reference argument.
        Action act = () => value.GetKeyFromString();
#pragma warning restore CS8604 // Possible null reference argument.

        // Assert
        if (expectedExceptionType != null)
        {
            act.Should().Throw<Exception>().And.GetType().Should().Be(expectedExceptionType);
        }
        else
        {
            act.Should().NotThrow();
        }
    }

    [Theory]
    [InlineData("A", "A", Scale.Major, Signature.None)]
    [InlineData("Bb", "B", Scale.Major, Signature.Flat)]
    [InlineData("F#", "F", Scale.Major, Signature.Sharp)]
    [InlineData("Cm", "C", Scale.Minor, Signature.None)]
    [InlineData("Dbm", "D", Scale.Minor, Signature.Flat)]
    public void GetKeyFromString_ShouldReturnCorrectKey_ForValidStrings(string value, string expectedNote, Scale expectedScale, Signature expectedSignature)
    {
        // Act
        var result = value.GetKeyFromString();

        // Assert
        result.Note.Should().Be(expectedNote);
        result.Scale.Should().Be(expectedScale);
        result.Signature.Should().Be(expectedSignature);
    }

    #endregion GetKeyFromString
}
