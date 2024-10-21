using Dapper;
using MixMate.Core.Entities;
using MixMate.Core.Enums;
using System.Data;

namespace MixMate.DataAccess.SqlHandlers;

public class KeyHandler : SqlMapper.TypeHandler<Key>
{
    public override Key Parse(object value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (value is not string input)
            throw new ArgumentException("Input valid is not parsable to a string value");

        // The first character is the note
        var note = input[..1];

        // The second part is "Minor" or "Major"
        var scale = input.Contains("Minor") ? Scale.Minor : Scale.Major;

        // The third part is whatever remains after extracting the scale
        var signatureString = input.Substring(note.Length + scale.ToString().Length);
        if (!Enum.TryParse(signatureString, out Signature signature))
            throw new ArgumentException("Unable to map input to a valid key signature");

        return new Key(note, scale, signature);
    }

    public override void SetValue(IDbDataParameter parameter, Key? value)
    {
        parameter.Value = value?.GetFullKey();
    }
}
