using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;

namespace MixMate.Core.Interfaces
{
    public interface IFileProcessingService
    {
        Task<List<Song>> ConvertFileLinesToSongsAsync(IBrowserFile file);
        Task<FileLoadResult> LoadSongsFromFiles(InputFileChangeEventArgs e);
    }
}