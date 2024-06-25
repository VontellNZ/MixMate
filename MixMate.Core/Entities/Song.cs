namespace MixMate.Core.Entities;

public struct Song(int id, string title, string artist, string? album, double bpm, string? genre, TimeSpan duration, Key key, DateTime dateAdded)
{
    public int Id { get; init; } = id;
    public string Title { get; init; } = title;
    public string Artist { get; init; } = artist;
    public string? Album { get; init; } = album;
    public double Bpm { get; init; } = bpm;
    public string? Genre { get; init; } = genre;
    public TimeSpan Duration { get; init; } = duration;
    public Key Key { get; init; } = key;
    public DateTime DateAdded { get; init; } = dateAdded;
}
