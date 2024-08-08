#nullable disable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;
using MixMate.Core.Interfaces;

namespace MixMate.Web.Components.Pages;

public partial class Home
{
    [Inject] private IFileProcessingService FileProcessingService { get; set; }
    [Inject] private ISongService SongService { get; set; }

    private const string _allowedFileExtension = ".txt";
    private const int _maxAllowedFiles = 1;
    private readonly List<string> errors = [];
    private List<Song> songs = [];

    private async Task LoadFiles(InputFileChangeEventArgs e)
    {
        errors.Clear();

        if (e.FileCount > _maxAllowedFiles)
        {
            errors.Add($"Error: Attempting to upload {e.FileCount} files, but only {_maxAllowedFiles} files are allowed.");
            return;
        }

        foreach (var file in e.GetMultipleFiles(_maxAllowedFiles))
        {
            try
            {
                var extension = Path.GetExtension(file.Name);
                if (!extension.Equals(_allowedFileExtension))
                {
                    errors.Add($"Error: Attempting to upload a {extension} file but only .txt files are allowed");
                    continue;
                }

                songs = await FileProcessingService.ConvertFileLinesToSongsAsync(file);

                await SongService.AddSongsAsync(songs);
            }
            catch (Exception ex)
            {
                errors.Add($"File: {file.Name}, Error: {ex.Message}");
                throw;
            }
        }
    }
}