using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;
using MixMate.Core.Enums;
using MixMate.Core.Extensions;
using System.Globalization;
using System.IO;

namespace MixMate.Core.Services;

public class FileProcessingService : IFileProcessingService
{
    public async Task<List<Song>> ConvertFileLinesToSongsAsync(IBrowserFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        List<Song> songs = [];

        using (var stream = file.OpenReadStream())
        using (var reader = new StreamReader(stream))
        {
            string? line;
            bool isFirstLine = true;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                if (isFirstLine)
                {
                    // Skip the header line

                    //TODO: Look into using header line for associating index with fields to increase stability of this code
                    isFirstLine = false;
                    continue;
                }

                var columns = line.Split('\t');

                if (columns.Length < 8) continue;

                int id = int.Parse(columns[0]);
                string title = columns[2];
                string artist = columns[3];
                string album = columns[4];
                //string genre = columns[5];
                double bpm = double.Parse(columns[5], CultureInfo.InvariantCulture);
                TimeSpan duration = TimeSpan.Parse(columns[6]);
                Key key = columns[7].GetKeyFromString();
                DateTime dateAdded = DateTime.Now;

                var song = new Song
                {
                    Id = id,
                    Title = title,
                    Artist = artist,
                    Album = album,
                    //Genre = genre,
                    Bpm = bpm,
                    Duration = duration,
                    Key = key,
                    DateAdded = dateAdded
                };

                songs.Add(song);
            }
        }

        return songs;
    }
}