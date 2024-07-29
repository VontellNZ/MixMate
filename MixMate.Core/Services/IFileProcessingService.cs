using Microsoft.AspNetCore.Components.Forms;
using MixMate.Core.Entities;

namespace MixMate.Core.Services
{
    public interface IFileProcessingService
    {
        Task<List<Song>> ConvertFileLinesToSongsAsync(IBrowserFile file);
    }
}