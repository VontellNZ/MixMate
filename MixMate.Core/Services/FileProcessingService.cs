﻿using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Constants;
using MixMate.Core.Entities;
using MixMate.Core.Extensions;
using MixMate.Core.Interfaces;
using System.Globalization;
using static MixMate.Core.Constants.TracklistHeaders;

namespace MixMate.Core.Services;

public class FileProcessingService : IFileProcessingService
{
    private string[] _columns = [];
    private Dictionary<string, int> _fields = [];

    public async Task<List<Song>> ConvertFileLinesToSongsAsync(IBrowserFile file)
    {
        ArgumentNullException.ThrowIfNull(file);

        List<Song> songs = [];

        using (var stream = file.OpenReadStream())
        using (var reader = new StreamReader(stream))
        {
            var firstLine = await reader.ReadLineAsync();
            if (firstLine == null) return songs; //Perhaps throw an exception here

            //Making sure these are empty at the start of processing a new file
            _fields = [];
            _columns = [];

            PopulateTrackFields(firstLine);

            string? line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                _columns = line.Split('\t');

                string title = GetFieldValueFromColumn(TrackTitle);
                string artist = GetFieldValueFromColumn(Artist);
                string album = GetFieldValueFromColumn(Album);
                string genre = GetFieldValueFromColumn(Genre);
                double bpm = double.Parse(GetFieldValueFromColumn(Bpm), CultureInfo.InvariantCulture);
                TimeSpan duration = TimeSpan.Parse(GetFieldValueFromColumn(Duration));
                Key key = GetFieldValueFromColumn(TracklistHeaders.Key).GetKeyFromString();

                var song = new Song
                {
                    Title = title,
                    Artist = artist,
                    Album = album,
                    Genre = genre,
                    Bpm = bpm,
                    Duration = duration,
                    Key = key,
                    DateAdded = DateTime.Now
                };

                songs.Add(song);
            }
        }

        return songs;
    }

    private void PopulateTrackFields(string firstLine)
    {
        _columns = firstLine.Split('\t');
        for (int i = 0; i < _columns.Length; i++)
        {
            _fields.Add(_columns[i], i);
        }
    }

    private string GetFieldValueFromColumn(string key) =>
        _fields.TryGetValue(key, out int value)
            ? _columns[value]
            : string.Empty;
}