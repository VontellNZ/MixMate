using MixMate.Core.Entities;
using MixMate.Core.Enums;

namespace MixMate.Core.Extensions;

public static class KeyExtensions
{
    private static readonly char _minorScaleSymbol = 'm';
    private static readonly List<char> _validNotes = ['A', 'B', 'C', 'D', 'E', 'F', 'G'];

    #region Key & Signature Dicts

    private static readonly Dictionary<(string note, Signature signature), CamelotScale> _minorKeys = new()
    {
        { ("A", Signature.Flat), new CamelotScale(1, "A") },
        { ("E", Signature.Flat), new CamelotScale(2, "A") },
        { ("B", Signature.Flat), new CamelotScale(3, "A") },
        { ("F", Signature.None), new CamelotScale(4, "A") },
        { ("C", Signature.None), new CamelotScale(5, "A") },
        { ("G", Signature.None), new CamelotScale(6, "A") },
        { ("D", Signature.None), new CamelotScale(7, "A") },
        { ("A", Signature.None), new CamelotScale(8, "A") },
        { ("E", Signature.None), new CamelotScale(9, "A") },
        { ("B", Signature.None), new CamelotScale(10, "A") },
        { ("F", Signature.Sharp), new CamelotScale(11, "A") },
        { ("D", Signature.Flat), new CamelotScale(12, "A") }
    };

    private static readonly Dictionary<(string note, Signature signature), CamelotScale> _majorKeys = new()
    {
        { ("B", Signature.None), new CamelotScale(1, "B") },
        { ("F", Signature.Sharp), new CamelotScale(2, "B") },
        { ("D", Signature.Flat), new CamelotScale(3, "B") },
        { ("A", Signature.Flat), new CamelotScale(4, "B") },
        { ("E", Signature.Flat), new CamelotScale(5, "B") },
        { ("B", Signature.Flat), new CamelotScale(6, "B") },
        { ("F", Signature.None), new CamelotScale(7, "B") },
        { ("C", Signature.None), new CamelotScale(8, "B") },
        { ("G", Signature.None), new CamelotScale(9, "B") },
        { ("D", Signature.None), new CamelotScale(10, "B") },
        { ("A", Signature.None), new CamelotScale(11, "B") },
        { ("E", Signature.None), new CamelotScale(12, "B") }
    };

    private static Dictionary<char, Signature> Signatures => new()
        {
            { '#', Signature.Sharp },
            { '♯', Signature.Sharp },
            { '♭', Signature.Flat },
            { 'b', Signature.Flat },
            { '♮', Signature.Natural }
        };

    #endregion Key & Signature Dicts

    public static CamelotScale GetCamelotScaleFromKey(this Key key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return key.Scale == Scale.Major
            ? _majorKeys[(key.Note, key.Signature)]
            : _minorKeys[(key.Note, key.Signature)];
    }

    public static Key GetKeyFromString(this string value)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(value);

        var valueArray = value.ToCharArray();

        //First letter 'should' always be the note - check it is valid
        if (!_validNotes.Contains(valueArray[0]))
            throw new FormatException($"Failed to parse key string. Invalid key note: {value}");

        var note = valueArray[0].ToString();
        if (valueArray.Length == 1)
            return new Key(note, Scale.Major, Signature.None);

        //After note check for a signature symbol - this is always after the note
        Signature signature = Signatures.ContainsKey(valueArray[1])
            ? Signatures[valueArray[1]]
            : Signature.None;

        if (valueArray.Length == 2)
        {
            // If key length is 2 and there is a signature symbol then
            // we must be in major scale as major doesn't use a symbol
            if (signature != Signature.None)
            {
                return new Key(note, Scale.Major, signature);
            }

            // No signature so we expect the second key char to be a minor scale symbol
            if (valueArray[1] == _minorScaleSymbol)
            {
                return new Key(note, Scale.Minor, signature);
            }

            // No signature and no scale
            // Something is wrong with the key string (value) or my code
            throw new FormatException($"Failed to parse key string. Unkown format for key: {value}");
        }
        else //Length of string is 3 or greater
        {
            // Key length of 3 means we should have signature and scale symbols
            if (signature != Signature.None && valueArray[2] == _minorScaleSymbol)
                return new Key(note, Scale.Minor, signature);

            // Something is wrong with the key string (value) or my code
            throw new FormatException($"Failed to parse key string. Unkown format for key: {value}");
        }
    }
}