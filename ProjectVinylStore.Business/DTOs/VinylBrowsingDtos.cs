using System.ComponentModel.DataAnnotations;

namespace ProjectVinylStore.Business.DTOs
{
    public class VinylRecordDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool InStock => StockQuantity > 0;
        public string CoverImageUrl { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
    }

    public class VinylSearchDto
    {
        [StringLength(100, ErrorMessage = "Search term must not exceed 100 characters")]
        public string? SearchTerm { get; set; }

        [StringLength(50, ErrorMessage = "Genre must not exceed 50 characters")]
        public string? Genre { get; set; }

        [StringLength(100, ErrorMessage = "Artist must not exceed 100 characters")]
        public string? Artist { get; set; }

        [Range(0, 10000, ErrorMessage = "Minimum price must be between 0 and 10000")]
        public decimal? MinPrice { get; set; }

        [Range(0, 10000, ErrorMessage = "Maximum price must be between 0 and 10000")]
        public decimal? MaxPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
        public int Page { get; set; } = 1;

        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;
    }

    public class PaginatedVinylResultDto
    {
        public IEnumerable<VinylRecordDto> Records { get; set; } = new List<VinylRecordDto>();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasNextPage => CurrentPage < TotalPages;
        public bool HasPreviousPage => CurrentPage > 1;
    }

    public class VinylDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool InStock => StockQuantity > 0;
        public string CoverImageUrl { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public AlbumDto Album { get; set; } = new AlbumDto();
    }

    public class AlbumDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
    }
}