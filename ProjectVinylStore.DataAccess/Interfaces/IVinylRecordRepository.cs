using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.DataAccess.Interfaces
{
    public interface IVinylRecordRepository : IGenericRepository<VinylRecord>
    {
        Task<IEnumerable<VinylRecord>> GetVinylsByAlbumIdAsync(int albumId);
        Task<IEnumerable<VinylRecord>> GetInStockVinylsAsync();
        Task<IEnumerable<VinylRecord>> GetVinylsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<VinylRecord?> GetVinylWithAlbumDetailsAsync(int id);
    }
}