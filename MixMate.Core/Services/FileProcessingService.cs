using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;
using MixMate.Core.Enums;

namespace MixMate.Core.Services;

public class FileProcessingService : IFileProcessingService
{
    public async Task<List<Song>> ConvertFileLinesToSongsAsync(IBrowserFile file)
    {
        //TODO: Figure out a way of validating if text file is correct format
        ArgumentNullException.ThrowIfNull(file);

        List<Song> songs = [];

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        try
        {
            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var fields = line.Split(',');

                if (fields.Length != 8)
                {
                    throw new FormatException("Invalid input line format");
                }

                var title = fields[0];
                var artist = fields[1];
                var album = fields[2];
                var genre = fields[3];
                var bpm = double.Parse(fields[4]);
                var duration = TimeSpan.Parse(fields[5]);
                //var key = fields[6];
                var key = new Key("F", Scale.Minor, Signature.None, new CamelotScale(4, "A")); //parse actual value into key
                var dateAdded = DateTime.Parse(fields[7]);

                //TODO: Validate songs before adding to list (and inserting into db once I've added that)
                songs.Add(new Song
                {
                    Title = title,
                    Artist = artist,
                    Album = album,
                    Genre = genre,
                    Duration = duration,
                    Key = key,
                    DateAdded = dateAdded
                });
            }
        }
        catch //more specific exception cases
        {
            //error handling
            //logging
            throw;
        }

        return songs;
    }
}