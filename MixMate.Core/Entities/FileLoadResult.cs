namespace MixMate.Core.Entities;

public class FileLoadResult(List<Song> songs, List<string> errors)
{
    public List<Song> Songs { get; set; } = songs;
    public List<string> Errors { get; set; } = errors;
}
