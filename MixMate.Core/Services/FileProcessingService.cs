using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;
using MixMate.Core.Extensions;
using MixMate.Core.Interfaces;
using System.Globalization;

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
            Dictionary<string, int> fields = [];

            while ((line = await reader.ReadLineAsync()) != null)
            {
                var columns = line.Split('\t');

                if (isFirstLine)
                {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        fields.Add(columns[i], i);
                    }

                    isFirstLine = false;
                    continue;
                }

                string title = columns[fields["Track Title"]];
                string artist = columns[fields["Artist"]];
                string album = columns[fields["Album"]];
                string genre = columns[fields["Genre"]];
                double bpm = double.Parse(columns[fields["BPM"]], CultureInfo.InvariantCulture);
                TimeSpan duration = TimeSpan.Parse(columns[fields["Time"]]);
                Key key = columns[fields["Key"]].GetKeyFromString();
                DateTime dateAdded = DateTime.Now;

                var song = new Song
                {
                    Title = title,
                    Artist = artist,
                    Album = album,
                    Genre = genre,
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