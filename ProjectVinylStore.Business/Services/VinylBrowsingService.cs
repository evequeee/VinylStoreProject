using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Exceptions;
using ProjectVinylStore.Business.Services;
using ProjectVinylStore.DataAccess.Interfaces;

namespace ProjectVinylStore.Business.Services
{
    public class VinylBrowsingService : IVinylBrowsingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VinylBrowsingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedVinylResultDto> GetVinylRecordsAsync(VinylSearchDto searchDto)
        {
            var allVinyls = await _unitOfWork.VinylRecords.GetAllAsync();
            var query = allVinyls.AsQueryable();

            if (searchDto.Page <= 0)
            {
                throw ApiException.BadRequest("Page number must be greater than 0", "INVALID_PAGE");
            }

            if (searchDto.PageSize <= 0 || searchDto.PageSize > 100)
            {
                throw ApiException.BadRequest("Page size must be between 1 and 100", "INVALID_PAGE_SIZE");
            }

            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
            {
                query = query.Where(v => v.Title.Contains(searchDto.SearchTerm) || v.Artist.Contains(searchDto.SearchTerm));
            }

            if (!string.IsNullOrEmpty(searchDto.Genre))
            {
                query = query.Where(v => v.Genre.Equals(searchDto.Genre, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(searchDto.Artist))
            {
                query = query.Where(v => v.Artist.Equals(searchDto.Artist, StringComparison.OrdinalIgnoreCase));
            }

            if (searchDto.MinPrice.HasValue)
            {
                query = query.Where(v => v.Price >= searchDto.MinPrice.Value);
            }

            if (searchDto.MaxPrice.HasValue)
            {
                query = query.Where(v => v.Price <= searchDto.MaxPrice.Value);
            }

            var totalCount = query.Count();
            var vinyls = query
                .Skip((searchDto.Page - 1) * searchDto.PageSize)
                .Take(searchDto.PageSize)
                .ToList();

            var vinylDtos = vinyls.Select(v => new VinylRecordDto
            {
                Id = v.Id,
                Title = v.Title,
                Artist = v.Artist,
                Genre = v.Genre,
                Price = v.Price,
                StockQuantity = v.StockQuantity,
                CoverImageUrl = v.CoverImageUrl,
                ReleaseDate = v.ReleaseDate
            });

            return new PaginatedVinylResultDto
            {
                Records = vinylDtos,
                TotalCount = totalCount,
                CurrentPage = searchDto.Page,
                PageSize = searchDto.PageSize
            };
        }

        public async Task<VinylDetailDto?> GetVinylDetailAsync(int vinylId)
        {
            var vinyl = await _unitOfWork.VinylRecords.GetVinylWithAlbumDetailsAsync(vinylId);
            if (vinyl == null)
            {
                throw ApiException.NotFound("Vinyl record", vinylId);
            }

            return new VinylDetailDto
            {
                Id = vinyl.Id,
                Title = vinyl.Title,
                Artist = vinyl.Artist,
                Genre = vinyl.Genre,
                Price = vinyl.Price,
                StockQuantity = vinyl.StockQuantity,
                CoverImageUrl = vinyl.CoverImageUrl,
                ReleaseDate = vinyl.ReleaseDate,
                Album = new AlbumDto
                {
                    Id = vinyl.Album.Id,
                    Title = vinyl.Album.Title,
                    Artist = vinyl.Album.Artist,
                    Genre = vinyl.Album.Genre,
                    ReleaseDate = vinyl.Album.ReleaseDate
                }
            };
        }

        public async Task<IEnumerable<VinylRecordDto>> SearchVinylRecordsAsync(string searchTerm)
        {
            var searchDto = new VinylSearchDto { SearchTerm = searchTerm, PageSize = 100 };
            var result = await GetVinylRecordsAsync(searchDto);
            return result.Records;
        }

        public async Task<IEnumerable<VinylRecordDto>> GetVinylRecordsByGenreAsync(string genre)
        {
            var vinyls = await _unitOfWork.VinylRecords.FindAsync(v => v.Genre == genre);
            return vinyls.Select(v => new VinylRecordDto
            {
                Id = v.Id,
                Title = v.Title,
                Artist = v.Artist,
                Genre = v.Genre,
                Price = v.Price,
                StockQuantity = v.StockQuantity,
                CoverImageUrl = v.CoverImageUrl,
                ReleaseDate = v.ReleaseDate
            });
        }

        public async Task<IEnumerable<VinylRecordDto>> GetVinylRecordsByArtistAsync(string artist)
        {
            var vinyls = await _unitOfWork.VinylRecords.FindAsync(v => v.Artist == artist);
            return vinyls.Select(v => new VinylRecordDto
            {
                Id = v.Id,
                Title = v.Title,
                Artist = v.Artist,
                Genre = v.Genre,
                Price = v.Price,
                StockQuantity = v.StockQuantity,
                CoverImageUrl = v.CoverImageUrl,
                ReleaseDate = v.ReleaseDate
            });
        }

        public async Task<IEnumerable<string>> GetAvailableGenresAsync()
        {
            var albums = await _unitOfWork.Albums.GetAllAsync();
            return albums.Select(a => a.Genre).Distinct().OrderBy(g => g);
        }

        public async Task<IEnumerable<string>> GetAvailableArtistsAsync()
        {
            var albums = await _unitOfWork.Albums.GetAllAsync();
            return albums.Select(a => a.Artist).Distinct().OrderBy(a => a);
        }
    }
}