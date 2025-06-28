using ProjectVinylStore.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This interface will define methods for interacting with vinyl records in the database.
// For example, methods for adding, updating, deleting, and retrieving vinyl records.

namespace ProjectVinylStore.DataAccess.Interfaces
{
    public interface IVinylRecordRepository : IGenericRepository<VinylRecord>
    {
        Task<IEnumerable<VinylRecord>> GetVinylsByAlbumIdAsync(int albumId);
        Task<IEnumerable<VinylRecord>> GetInStockVinylsAsync(string artist);
        Task<IEnumerable<VinylRecord>> GetVinylsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<VinylRecord> GetVinylWithAlbumDetailsAsync(int id);
    }
}
