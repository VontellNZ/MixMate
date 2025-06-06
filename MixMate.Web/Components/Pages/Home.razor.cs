#nullable disable

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;
using MixMate.Core.Interfaces;
using MixMate.Web.Interfaces;
using MudBlazor;
using System.Collections.ObjectModel;

namespace MixMate.Web.Components.Pages;

public partial class Home
{    
    [Inject] private IFileProcessingService FileProcessingService { get; set; }
    [Inject] private IMixMateClient MixMateClient { get; set; }
    [Inject] private IMixingService MixingService { get; set; }
    [Inject] private ILogger<Home> Logger { get; set; }
    private Song? MainSong
    {
        get => _mainSong;
        set
        {
            _mainSong = value;
            GetSuggestedSongs(SelectedMixingTechnique);
        }
    }
    private string MainSongCardText
    {
        get
        {
            if (MainSong == null) return "Select a song from below to start mixing!";

            return $"Main Song: {MainSong?.Title} - {MainSong?.Artist}";
        }
    }
    private string SelectedMixingTechnique
    {
        get => _selectedMixingTechnique;
        set
        {
            _selectedMixingTechnique = value;
            GetSuggestedSongs(value);
        }
    }

    private const int _maxAllowedFiles = 1;
    private readonly List<string> _errors = [];
    private List<string> _availableMixingTechniqueNames = [];
    private ObservableCollection<Song> _songs = [];
    private List<Song> _suggestedSongs = [];
    private Song? _mainSong;
    private string _selectedMixingTechnique;

    protected override async Task OnInitializedAsync()
    {
        var songs = await MixMateClient.GetAllSongsAsync();
        _songs = new ObservableCollection<Song>(songs);

        _availableMixingTechniqueNames.Clear();
        _availableMixingTechniqueNames = MixingService.GetAvailableMixingTechniqueNames();
        SelectedMixingTechnique = _availableMixingTechniqueNames.FirstOrDefault();

        await base.OnInitializedAsync();
    }

    private async Task LoadFiles(InputFileChangeEventArgs e)
    {
        _errors.Clear();

        if (e.FileCount > _maxAllowedFiles)
        {
            Logger.LogWarning("Attempting to upload {FileCount} files, but only {MaxFiles} files are allowed", e.FileCount, _maxAllowedFiles);
            _errors.Add($"Error: Attempting to upload {e.FileCount} files, but only {_maxAllowedFiles} files are allowed.");
            return;
        }

        var fileLoadResult = await FileProcessingService.LoadSongsFromFiles(e);
        await MixMateClient.AddSongsAsync(fileLoadResult.Songs);

        foreach (var song in fileLoadResult.Songs)
        {
            _songs.Add(song);
        }

        _errors.AddRange(fileLoadResult.Errors);
    }

    private void SetMainSong(DataGridRowClickEventArgs<Song> selectedSong) 
        => MainSong = selectedSong.Item;

    private void GetSuggestedSongs(string mixingTechniqueName)
    {
        if (MainSong == null) return;

        _suggestedSongs = MixingService.GetSuggestedSongs(mixingTechniqueName, (Song)MainSong, _songs.ToList());
    }

    private string RowStyleFunc(Song rowSong, int index)
    {
        if (rowSong.Equals(_mainSong))
        {
            return "background-color:PaleGoldenRod";
        }
        if (_suggestedSongs.Contains(rowSong))
        {
            return "background-color:LightGreen";
        }

        return "background-color:white";
    }
}