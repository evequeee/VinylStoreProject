using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.DataAccess.Entities;
using ProjectVinylStore.DataAccess.Interfaces;

namespace ProjectVinylStore.DataAccess.Repositories
{
    public class VinylRecordRepository : GenericRepository<VinylRecord>, IVinylRecordRepository
    {
        public VinylRecordRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VinylRecord>> GetVinylsByAlbumIdAsync(int albumId)
        {
            return await _context.VinylRecords
                .Where(v => v.AlbumId == albumId)
                .ToListAsync();
        }

        public async Task<IEnumerable<VinylRecord>> GetInStockVinylsAsync()
        {
            return await _context.VinylRecords
                .Where(v => v.StockQuantity > 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<VinylRecord>> GetVinylsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.VinylRecords
                .Where(v => v.Price >= minPrice && v.Price <= maxPrice)
                .ToListAsync();
        }

        public async Task<VinylRecord?> GetVinylWithAlbumDetailsAsync(int id)
        {
            return await _context.VinylRecords
                .Include(v => v.Album)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}