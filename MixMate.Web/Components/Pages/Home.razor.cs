#nullable disable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;
using MixMate.Core.Interfaces;
using MudBlazor;
using System.Collections.ObjectModel;

namespace MixMate.Web.Components.Pages;

public partial class Home
{
    [Inject] private IFileProcessingService FileProcessingService { get; set; }
    [Inject] private ISongService SongService { get; set; }

    private const int _maxAllowedFiles = 1;
    private readonly List<string> Errors = [];
    private ObservableCollection<Song> _songs = [];
    private Song? _mainSong;

    protected override async Task OnInitializedAsync()
    {
        var songs = await SongService.GetAllSongsAsync();
        this._songs = new ObservableCollection<Song>(songs);

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

        var songs = await FileProcessingService.LoadSongsFromFiles(e);
        foreach (var song in songs)
        {
            this._songs.Add(song);
        }
    }

    private void SetMainSong(DataGridRowClickEventArgs<Song> selectedSong) 
        => _mainSong = selectedSong.Item;
}