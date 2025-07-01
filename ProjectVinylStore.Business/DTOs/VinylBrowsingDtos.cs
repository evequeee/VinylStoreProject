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
        public string? SearchTerm { get; set; }
        public string? Genre { get; set; }
        public string? Artist { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int Page { get; set; } = 1;
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