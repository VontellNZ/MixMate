namespace MixMate.Core.Entities;

public class SongInput
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string? Album { get; set; }
    public string? Genre { get; set; }
    public double Bpm { get; set; }
    public TimeSpan Duration { get; set; }
    public KeyInput Key { get; set; } = null!;
    public DateTime? DateAdded { get; set; }
}

public class KeyInput
{
    public string Note { get; set; } = string.Empty;
    public string Scale { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}

public class AddSongsPayload
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int SongsAdded { get; set; }
}
