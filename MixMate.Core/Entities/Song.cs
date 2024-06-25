namespace MixMate.Core.Entities;

public struct Song(int id, string title, string artist, string? album, string? genre, double bpm, TimeSpan duration, Key key, DateTime dateAdded)
{
    public int Id { get; init; } = id;
    public string Title { get; init; } = title;
    public string Artist { get; init; } = artist;
    public string? Album { get; init; } = album;
    public string? Genre { get; init; } = genre;
    public double Bpm { get; init; } = bpm;
    public TimeSpan Duration { get; init; } = duration;
    public Key Key { get; init; } = key;
    public DateTime DateAdded { get; init; } = dateAdded;
}
