using ProjectVinylStore.Business.DTOs;

namespace ProjectVinylStore.Business.Services
{
    public interface IVinylBrowsingService
    {
        // Browse Vinyl Records
        Task<PaginatedVinylResultDto> GetVinylRecordsAsync(VinylSearchDto searchDto);
        Task<VinylDetailDto?> GetVinylDetailAsync(int vinylId);
        Task<IEnumerable<VinylRecordDto>> SearchVinylRecordsAsync(string searchTerm);
        Task<IEnumerable<VinylRecordDto>> GetVinylRecordsByGenreAsync(string genre);
        Task<IEnumerable<VinylRecordDto>> GetVinylRecordsByArtistAsync(string artist);
        Task<IEnumerable<string>> GetAvailableGenresAsync();
        Task<IEnumerable<string>> GetAvailableArtistsAsync();
    }
}