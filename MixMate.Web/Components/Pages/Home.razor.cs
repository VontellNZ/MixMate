#nullable disable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;
using MixMate.Core.Interfaces;
using System.Collections.ObjectModel;

namespace MixMate.Web.Components.Pages;

public partial class Home
{
    [Inject] private IFileProcessingService FileProcessingService { get; set; }
    [Inject] private ISongService SongService { get; set; }

    private const string _allowedFileExtension = ".txt";
    private const int _maxAllowedFiles = 1;
    private readonly List<string> Errors = [];
    private ObservableCollection<Song> Songs = [];

    protected override async Task OnInitializedAsync()
    {
        var songs = await SongService.GetAllSongsAsync();
        Songs = new ObservableCollection<Song>(songs);

        await base.OnInitializedAsync();
    }

    private async Task LoadFiles(InputFileChangeEventArgs e)
    {
        Errors.Clear();

        if (e.FileCount > _maxAllowedFiles)
        {
            Errors.Add($"Error: Attempting to upload {e.FileCount} files, but only {_maxAllowedFiles} files are allowed.");
            return;
        }

        foreach (var file in e.GetMultipleFiles(_maxAllowedFiles))
        {
            try
            {
                var extension = Path.GetExtension(file.Name);
                if (!extension.Equals(_allowedFileExtension))
                {
                    Errors.Add($"Error: Attempting to upload a {extension} file but only .txt files are allowed");
                    continue;
                }

                var songs = await FileProcessingService.ConvertFileLinesToSongsAsync(file);

                await SongService.AddSongsAsync(songs);

                foreach (var song in songs)
                {
                    Songs.Add(song);
                }
            }
            catch (Exception ex)
            {
                Errors.Add($"File: {file.Name}, Error: {ex.Message}");
                throw;
            }
        }
    }
}