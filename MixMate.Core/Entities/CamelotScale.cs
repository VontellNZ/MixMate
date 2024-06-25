namespace MixMate.Core.Entities;

public class CamelotScale(int number, string letter)
{
    public int Number { get; set; } = number;
    public string Letter { get; set; } = letter.ToUpper();

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        return obj is CamelotScale item
            && Number.Equals(item.Number) && Letter.Equals(item.Letter);
    }

    public override int GetHashCode() => base.GetHashCode();
}