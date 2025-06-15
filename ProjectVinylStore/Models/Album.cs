namespace ProjectVinylStore.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Artist { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; } = string.Empty;
        public bool InStock { get; set; }
    }
}