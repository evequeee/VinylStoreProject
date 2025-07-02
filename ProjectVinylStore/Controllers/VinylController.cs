using Microsoft.AspNetCore.Mvc;
using ProjectVinylStore.Business.DTOs;
using ProjectVinylStore.Business.Exceptions;
using ProjectVinylStore.Business.Services;
using ProjectVinylStore.Filters;

namespace ProjectVinylStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ValidateModel]
    public class VinylController : ControllerBase
    {
        private readonly IVinylBrowsingService _vinylBrowsingService;

        public VinylController(IVinylBrowsingService vinylBrowsingService)
        {
            _vinylBrowsingService = vinylBrowsingService;
        }

        /// Browse vinyl records with filtering and pagination
        /// <returns>Paginated list of vinyl records</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PaginatedVinylResultDto>>> GetVinyls([FromQuery] VinylSearchDto searchDto)
        {
            var result = await _vinylBrowsingService.GetVinylRecordsAsync(searchDto);
            return Ok(ApiResponse<PaginatedVinylResultDto>.SuccessResult(result, "Vinyl records retrieved successfully"));
        }

        /// Get vinyl record details by ID
        /// <returns>Detailed vinyl record information</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<VinylDetailDto>>> GetVinyl(int id)
        {
            var vinyl = await _vinylBrowsingService.GetVinylDetailAsync(id);
            
            if (vinyl == null)
            {
                throw ApiException.NotFound("Vinyl record", id);
            }

            return Ok(ApiResponse<VinylDetailDto>.SuccessResult(vinyl, "Vinyl record retrieved successfully"));
        }

        /// Search vinyl records by search term
        /// <returns>List of matching vinyl records</returns>
        [HttpGet("search")]
        public async Task<ActionResult<ApiResponse<IEnumerable<VinylRecordDto>>>> SearchVinyls([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                throw ApiException.BadRequest("Search term cannot be empty", "INVALID_SEARCH_TERM");
            }

            var result = await _vinylBrowsingService.SearchVinylRecordsAsync(searchTerm);
            return Ok(ApiResponse<IEnumerable<VinylRecordDto>>.SuccessResult(result, "Search completed successfully"));
        }

        /// Get vinyl records by genre
        /// <returns>List of vinyl records in the specified genre</returns>
        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<VinylRecordDto>>>> GetVinylsByGenre(string genre)
        {
            var result = await _vinylBrowsingService.GetVinylRecordsByGenreAsync(genre);
            return Ok(ApiResponse<IEnumerable<VinylRecordDto>>.SuccessResult(result, $"Vinyl records in '{genre}' genre retrieved successfully"));
        }

        /// Get vinyl records by artist
        /// <returns>List of vinyl records by the specified artist</returns>
        [HttpGet("artist/{artist}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<VinylRecordDto>>>> GetVinylsByArtist(string artist)
        {
            var result = await _vinylBrowsingService.GetVinylRecordsByArtistAsync(artist);
            return Ok(ApiResponse<IEnumerable<VinylRecordDto>>.SuccessResult(result, $"Vinyl records by '{artist}' retrieved successfully"));
        }

        /// Get available genres
        /// <returns>List of available genres</returns>
        [HttpGet("genres")]
        public async Task<ActionResult<ApiResponse<IEnumerable<string>>>> GetGenres()
        {
            var genres = await _vinylBrowsingService.GetAvailableGenresAsync();
            return Ok(ApiResponse<IEnumerable<string>>.SuccessResult(genres, "Genres retrieved successfully"));
        }

        /// Get available artists
        /// <returns>List of available artists</returns>
        [HttpGet("artists")]
        public async Task<ActionResult<ApiResponse<IEnumerable<string>>>> GetArtists()
        {
            var artists = await _vinylBrowsingService.GetAvailableArtistsAsync();
            return Ok(ApiResponse<IEnumerable<string>>.SuccessResult(artists, "Artists retrieved successfully"));
        }
    }
}