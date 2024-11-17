using MixMate.Core.Enums;
using MixMate.Core.Extensions;

namespace MixMate.Core.Entities;

public class Key
{
    public string Note { get; set; }
    public Scale Scale { get; set; }
    public Signature Signature { get; set; }
    public CamelotScale CamelotScale { get; set; }

    public Key(string note, Scale scale, Signature signature, CamelotScale camelotScale)
    {
        ArgumentNullException.ThrowIfNull(note);

        Note = note.ToUpper();
        Scale = scale;
        Signature = signature;
        CamelotScale = camelotScale;
    }

    public Key(string note, Scale scale, Signature signature)
    {
        ArgumentNullException.ThrowIfNull(note);

        Note = note.ToUpper();
        Scale = scale;
        Signature = signature;
        CamelotScale = this.GetCamelotScaleFromKey();
    }

    public string GetFullKey() => Note + Scale.ToString() + Signature.ToString();
    public string GetFullKeyForDisplay() 
        => $"{Note} {(Signature != Signature.None ? Signature.ToString() : string.Empty)} {Scale}";
}
