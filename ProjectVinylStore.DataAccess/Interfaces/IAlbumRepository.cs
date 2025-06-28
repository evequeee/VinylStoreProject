using ProjectVinylStore.DataAccess.Entities;

namespace ProjectVinylStore.DataAccess.Interfaces
{
    public interface IAlbumRepository : IGenericRepository<Album>
    {
        Task<IEnumerable<Album>> GetAlbumsByGenreAsync(string genre);
        Task<IEnumerable<Album>> GetAlbumsByArtistAsync(string artist);
        Task<IEnumerable<Album>> GetAlbumsByReleaseDateAsync(DateTime releaseDate);
        Task<IEnumerable<Album>> GetLatestAlbumsAsync(int count);
    }
}