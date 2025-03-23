namespace MixMate.Web.Services;

public class SongDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string? Album { get; set; }
    public double BPM { get; set; }
    public string? Genre { get; set; }
    public string Key { get; set; } = string.Empty;
    public double Duration { get; set; }
    public DateTime DateAdded { get; set; }
}
