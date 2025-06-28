using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.DataAccess.Entities;
using ProjectVinylStore.DataAccess.Interfaces;
using System.Data;

namespace ProjectVinylStore.DataAccess.Repositories
{
    public class AlbumRepository : GenericRepository<Album>, IAlbumRepository
    {
        public AlbumRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Album>> GetAlbumsByGenreAsync(string genre)
        {
            return await _context.Albums
                .Where(a => a.Genre == genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByArtistAsync(string artist)
        {
            return await _context.Albums
                .Where(a => a.Artist == artist)
                .ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetLatestAlbumsAsync(int count)
        {
            return await _context.Albums
                .OrderByDescending(a => a.ReleaseDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Album>> GetAlbumsByReleaseDateAsync(DateTime releaseDate)
        {
            return await _context.Albums
                .Where(a => a.ReleaseDate.Date == releaseDate.Date)
                .ToListAsync();
        }
    }
}