﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using MixMate.Core.Constants;
using MixMate.Core.Entities;
using MixMate.Core.Extensions;
using MixMate.Core.Interfaces;
using System.Globalization;
using static MixMate.Core.Constants.TracklistHeaders;

namespace MixMate.Core.Services;

public class FileProcessingService(ILogger<FileProcessingService> logger) : IFileProcessingService
{
    private const int MaxAllowedFiles = 1;
    private const string AllowedFileExtension = ".txt";
    private readonly ILogger<FileProcessingService> _logger = logger;
    private string[] _columns = [];
    private Dictionary<string, int> _fields = [];

    public async Task<FileLoadResult> LoadSongsFromFiles(InputFileChangeEventArgs e)
    {
        var songs = new List<Song>();
        var errors = new List<string>();

        foreach (var file in e.GetMultipleFiles(MaxAllowedFiles))
        {
            try
            {
                var extension = Path.GetExtension(file.Name);
                if (!extension.Equals(AllowedFileExtension))
                {
                    _logger.LogWarning("Attempting to upload a {Extension} file but only .txt files are allowed", extension);
                    errors.Add($"Attempting to upload a {extension} file but only .txt files are allowed");
                    continue;
                }

                var processedSongs = await ConvertFileLinesToSongsAsync(file);
                songs.AddRange(processedSongs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing file {FileName}", file.Name);
                errors.Add($"Error occurred while processing file {file.Name}");
            }
        }

        return new FileLoadResult(songs, errors);
    }

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