#nullable disable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;
using MixMate.Core.Interfaces;
using MudBlazor;
using System.Collections.ObjectModel;
using static MudBlazor.CategoryTypes;

namespace MixMate.Web.Components.Pages;

public partial class Home
{
    public Song? MainSong
    {
        get => _mainSong;
        set
        {
            _mainSong = value;
            GetSuggestedSongs();
        }
    }
    [Inject] private IFileProcessingService FileProcessingService { get; set; }
    [Inject] private ISongService SongService { get; set; }
    [Inject] private IMixingService MixingService { get; set; }

    private const int _maxAllowedFiles = 1;
    private readonly List<string> Errors = [];
    private ObservableCollection<Song> _songs = [];
    private List<Song> _suggestedSongs = [];
    private Song? _mainSong;

    protected override async Task OnInitializedAsync()
    {
        var songs = await SongService.GetAllSongsAsync();
        _songs = new ObservableCollection<Song>(songs);

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

        var fileLoadResult = await FileProcessingService.LoadSongsFromFiles(e);
        foreach (var song in fileLoadResult.Songs)
        {
            _songs.Add(song);
        }

        Errors.AddRange(fileLoadResult.Errors);
    }

    private void SetMainSong(DataGridRowClickEventArgs<Song> selectedSong) 
        => MainSong = selectedSong.Item;

    private void GetSuggestedSongs()
    {
        if (MainSong == null) return;

        _suggestedSongs = MixingService.GetSuggestedSongs("SmoothMixingTechnique", (Song)MainSong, _songs.ToList());
    }

    private string RowStyleFunc(Song arg1, int index)
    {
        if (_suggestedSongs.Contains(arg1))
        {
            return "background-color:LightGreen";
        }

        return "background-color:white";
    }
}