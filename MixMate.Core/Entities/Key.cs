using MixMate.Core.Enums;

namespace MixMate.Core.Entities;

public class Key(string note, Scale scale, Signature signature, CamelotScale camelotScale)
{
    public string Note { get; set; } = note.ToUpper();
    public Scale Scale { get; set; } = scale;
    public Signature Signature { get; set; } = signature;
    public CamelotScale CamelotScale { get; set; } = camelotScale;

    public string GetFullKey() => Note + Scale.ToString() + Signature.ToString();
}
