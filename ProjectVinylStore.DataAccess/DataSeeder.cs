using Microsoft.EntityFrameworkCore;
using ProjectVinylStore.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectVinylStore.DataAccess
{
    public class DataSeeder
    {
        private readonly AppDbContext _context;
        private readonly Random _random = new Random();

        public DataSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAllAsync()
        {
            await SeedUsersAsync();
            await SeedAlbumsAsync();
            await SeedVinylRecordsAsync();
            await SeedOrdersAsync();
        }

        public async Task SeedUsersAsync(int count = 10)
        {
            if (await _context.Set<ApplicationUser>().AnyAsync())
                return;

            var users = new List<ApplicationUser>();

            for (int i = 0; i < count; i++)
            {
                users.Add(new ApplicationUser 
                {
                    FirstName = $"User{i}",
                    LastName = $"Lastname{i}",
                    Email = $"user{i}@gmail.com",
                    UserName = $"user{i}@gmail.com",
                    EmailConfirmed = true,
                    Orders = new List<Order>()
                });
            }

            await _context.AddRangeAsync(users);
            await _context.SaveChangesAsync();
        }

        public async Task SeedAlbumsAsync(int count = 20)
        {
            if (await _context.Set<Album>().AnyAsync())
                return;

            var genres = new[] { "Rock", "Hard Rock", "Psychedelic Rock", "Metal", "Hard Metal", "Doom Metal", "Thrash Metal", "Nu-Metal", "Grunge", "Alternative Rock", "Indie Rock" };
            var artists = new[]
            {
                "Led Zeppelin", "Black Sabbath", "Pink Floyd", "AC/DC", "Nirvana", "Radiohead", "Metallica", "Queen", "Slipknot", "Dio",
                "The Rolling Stones", "Guns N' Roses", "Pearl Jam", "Alice in Chains", "Soundgarden", "Tool", "System of a Down", "Foo Fighters",
                "The Smashing Pumpkins", "Rage Against the Machine", "Korn", "Speedy Ortiz", "Deftones", "Muse", "The Cure", "The Who", "The Doors",
                "The Smiths", "Joy Division", "The Pixies", "Sword", "Type O Negative", "Duster", "Megadeth", "Motörhead"
            };
            var albums = new List<Album>();

            for (int i = 1; i <= count; i++)
            {
                var artist = artists[_random.Next(artists.Length)];
                albums.Add(new Album
                {
                    Title = $"Album {i}",
                    Artist = artist,
                    Genre = genres[_random.Next(genres.Length)],
                    ReleaseDate = DateTime.SpecifyKind(
                        new DateTime(
                        _random.Next(1960, 2025),
                        _random.Next(1, 13),
                        _random.Next(1, 29)
                    ),
                    DateTimeKind.Utc),
                    AlbumId = i
                });
            }

            await _context.AddRangeAsync(albums);
            await _context.SaveChangesAsync();
        }

        public async Task SeedVinylRecordsAsync()
        {
            if (await _context.Set<VinylRecord>().AnyAsync())
                return;

            var albums = await _context.Set<Album>().ToListAsync();
            var vinylRecords = new List<VinylRecord>();

            foreach (var album in albums)
            {
                int variants = _random.Next(1, 5);

                for (int i = 1; i <= variants; i++)
                {
                    string edition = variants > 1 ? GetRandomEdition(i) : string.Empty;
                    string title = string.IsNullOrEmpty(edition) ? album.Title : $"{album.Title} ({edition})";

                    vinylRecords.Add(new VinylRecord
                    {
                        Title = title,
                        Price = Math.Round(_random.Next(1000, 5000) / 100m, 2),
                        StockQuantity = _random.Next(1, 100),
                        CoverImageUrl = $"https://example.com/images/{album.Id}_{i}.jpg",
                        AlbumId = album.Id,
                        Album = album
                    });
                }
            }

            await _context.AddRangeAsync(vinylRecords);
            await _context.SaveChangesAsync();
        }

        public async Task SeedOrdersAsync(int count = 30)
        {
            if (await _context.Set<Order>().AnyAsync())
                return;

            var users = await _context.Set<ApplicationUser>().ToListAsync();
            var vinylRecords = await _context.Set<VinylRecord>().ToListAsync();
            var orders = new List<Order>();

            for (int i = 1; i <= count; i++)
            {
                var user = users[_random.Next(users.Count)];
                var record = vinylRecords[_random.Next(vinylRecords.Count)];

                var orderDate = DateTime.UtcNow.AddDays(-_random.Next(1, 365));

                orders.Add(new Order
                {
                    ProductName = record.Title,
                    UserId = user.Id,
                    User = user,
                    OrderDate = orderDate,
                    TotalAmount = record.Price
                });
            }

            await _context.AddRangeAsync(orders);
            await _context.SaveChangesAsync();
        }

        private string GetRandomEdition(int variantNumber)
        {
            var editions = new[]
            { "Standard", "Limited Edition", "Collector's Edition", "Deluxe Edition", "Anniversary Edition", "Remastered", "Live Recording" };

            return variantNumber == 1 ? string.Empty : editions[_random.Next(editions.Length)];
        }
    }
}